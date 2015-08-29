using JoyfulColours.Animations;
using JoyfulColours.Library;
using JoyfulColours.Logic;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    /// <summary>
    /// Base class for visible and / or collidable models in a 3D scene.
    /// </summary>
    public class Model : Element
    {
        public ModelTemplate Template { get; }

        public Dictionary<string, Node> Nodes { get; } = new Dictionary<string, Node>();
        public List<Equipment> Equipments { get; } = new List<Equipment>();

        public ScriptScope Script { get; set; }

        public Model(ModelTemplate template)
        {
            Template = template;

            // Load geometries
            if (template.Skeleton != null)
                // Produce a visual tree if skeleton is defined
                Children.Add(LoadNode(template.Skeleton.Root));
            else
                // Else, just load all geometries into the master model
                LoadGeometries(template);

            // Load equipments
            foreach (EquipmentTemplate et in template.Equipments)
                Equip(new Equipment(et));

            // Setup and execute script
            ScriptScope script = template.Code.Load("model", this);

            // Add UIs and execute scripts within block
            foreach (var item in template.UITemplates.Values)
            {
                // TODO: Remove ugly ref
                item.CreateVisual3D(this, ref script);
            }
            Script = script;
        }

        private Node LoadNode(NodeTemplate nt)
        {
            Node node = new Node(nt);
            node.Content = Template.Model.Geometries[nt.Name];
            Nodes[nt.Name] = node;
            foreach (NodeTemplate child in nt.Children)
            {
                node.Children.Add(LoadNode(child));
            }
            return node;
        }

        private void LoadGeometries(ModelTemplate template)
        {
            // Add geometries
            foreach (GeometryModel3D model in template.Model.Geometries.Values)
            {
                ModelVisual3D m = new ModelVisual3D();
                m.Content = model;
                Children.Add(m);
            }
        }

        public event EventHandler<RayHitTestResult> Click;

        public virtual void OnClick(RayHitTestResult res)
        {
            Click?.Invoke(this, res);
        }
        
        public void Equip(Equipment equipment)
        {
            Equipments.Add(equipment);

            if (equipment.Parent != null)
                equipment.Parent.Unequip(equipment);
            equipment.Parent = this;
            foreach (Addon addon in equipment.Addons)
                Nodes[addon.Node].Equip(addon);
        }

        public void Unequip(Equipment equipment)
        {
            Equipments.Remove(equipment);

            foreach (Addon addon in equipment.Addons)
                Nodes[addon.Template.Node].Unequip(addon);
        }

        public event EventHandler<Interaction> Interacted;

        protected virtual void OnInteracted(Interaction i)
        {
            Interacted?.Invoke(this, i);
        }

        public void Interact(Interaction i)
        {
            OnInteracted(i);
        }

        public override string ToString() => $"{nameof(Model)} \"{Template.ID}\"";
    }
}

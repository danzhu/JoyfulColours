using JoyfulColours.Interface;
using JoyfulColours.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace JoyfulColours.Elements
{
    public class Scene : Element
    {
        // Properties
        public string ID { get; }
        public string Name { get; set; }
        public Dimension3D Dimension { get; set; }

        // Members
        Model[,,] models;
        HashSet<Lamp> lamps = new HashSet<Lamp>();
        List<Actor> actors = new List<Actor>();

        Dictionary<string, Element> namedElements = new Dictionary<string, Element>();

        public Scene(Loader l)
        {
            ID = l.ID;

            foreach (Instruction i in l.Parse())
                Load(l, i);
        }

        public virtual void Load(Loader l, Instruction i)
        {
            switch (i.Type)
            {
                case "name":
                    Name = i.String();
                    break;
                case "dim":
                    int width = i.Int(), height = i.Int(), length = i.Int();
                    Dimension = new Dimension3D(width, height, length);
                    models = new Model[width, height, length];
                    break;
                case "m": // Model
                    {
                        Position3D pos = i.Position3D();
                        ModelTemplate mt = Game.Get<ModelTemplate>(i.String());
                        Model m = new Model(mt);
                        AddModel(m, pos);
                        ProcessModelArgs(i, m);
                    }
                    break;
                case "r": // Range (multiple models)
                    {
                        Position3D origin = i.Position3D();
                        Dimension3D dim = i.Dimension3D();
                        ModelTemplate mt = Game.Get<ModelTemplate>(i.String());
                        origin.ForEachInRange(dim, (loc) =>
                        {
                            Model m = new Model(mt);
                            AddModel(m, loc);
                        });
                    }
                    break;
                case "a": // Actor
                    {
                        Position3D pos = i.Position3D();
                        ActorTemplate at = Game.Get<ActorTemplate>(i.String());
                        Actor actor = new Actor(at);
                        AddActor(actor);
                        ProcessModelArgs(i, actor);
                    }
                    break;
                case "dl": // Directional light
                    {
                        Light light = new DirectionalLight(i.Color(), i.Vector3D());
                        Lamp ls = new Lamp(light);
                        AddLamp(ls);
                        if (i.HasNext)
                            namedElements[i.String()] = ls;
                    }
                    break;
                case "pl": // Point light
                    {
                        Light light = new PointLight(i.Color(), i.Point3D());
                        Lamp ls = new Lamp(light);
                        AddLamp(ls);
                        if (i.HasNext)
                            namedElements[i.String()] = ls;
                    }
                    break;
                case "al": // Ambient light
                    {
                        Light light = new AmbientLight(i.Color());
                        Lamp ls = new Lamp(light);
                        AddLamp(ls);
                        if (i.HasNext)
                            namedElements[i.String()] = ls;
                    }
                    break;
            }
        }

        private void ProcessModelArgs(Instruction i, Model m)
        {
            while (i.HasNext)
            {
                switch (i.String())
                {
                    case "-id":
                        namedElements[i.String()] = m;
                        break;
                    case "-dir":
                        m.Direction = new Direction(i.Int());
                        break;
                }
            }
        }

        public Model this[Position3D location]
        {
            get { return models[location.X, location.Y, location.Z]; }
            set { models[location.X, location.Y, location.Z] = value; }
        }

        public void MoveModelTo(Model model, Position3D location)
        {
            ClearModel(model);
            PlaceModel(model, location);
        }

        public void PlaceModel(Model model, Position3D location)
        {
            // Set new location (for visual update)
            model.Position = location;
            // Set new area
            location.ForEachInRange(model.Template.Dimension, (l) =>
            {
                Model m = this[l];
                if (m != null)
                {
                    // Remove object being replaced
                    RemoveModel(m);
                }
                this[l] = model;
            });
        }

        public void ClearModel(Model model)
        {
            // Clear area the model was before
            model.Position.ForEachInRange(model.Template.Dimension, (l) => this[l] = null);
        }

        public void AddModel(Model Model, Position3D location)
        {
            PlaceModel(Model, location);
            Children.Add(Model);
        }

        public void RemoveModel(Model Model)
        {
            ClearModel(Model);
            Children.Remove(Model);
        }

        public void AddActor(Actor actor)
        {
            actors.Add(actor);
            Children.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
            Children.Remove(actor);
        }

        public void AddLamp(Lamp lamp)
        {
            lamps.Add(lamp);
            Children.Add(lamp);
        }

        public void RemoveLamp(Lamp lamp)
        {
            lamps.Remove(lamp);
            Children.Remove(lamp);
        }
        
        public bool CanPassThrough(Position3D pos)
        {
            Model b = this[pos];
            return b == null || !b.Template.IsCollidable;
        }
        
        public Element this[string id]
        {
            get { return namedElements[id]; }
        }

        public override string ToString() => $"{nameof(Scene)} \"{ID}\"  ({Name})";
    }
}

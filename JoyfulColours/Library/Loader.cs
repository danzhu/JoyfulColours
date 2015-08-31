using JoyfulColours.Animations;
using JoyfulColours.Elements;
using JoyfulColours.Interface;
using JoyfulColours.Logic;
using JoyfulColours.Procedures;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Library
{
    public delegate object Constructor(Loader l);

    /// <summary>
    /// Converts a file into data used by the game engine.
    /// </summary>
    public abstract class Loader
    {
        static Dictionary<string, Constructor> constructors
            = new Dictionary<string, Constructor>();

        static HashSet<string> immediateLoadingExtensions
            = new HashSet<string>();

        public static void RegisterType(string ext, Constructor ctor,
            bool loadImmediately = false)
        {
            constructors.Add(ext, ctor);
            if (loadImmediately)
                immediateLoadingExtensions.Add(ext);
        }

        public static void Initialize()
        {
            RegisterType(".scn", l => new Scene(l));
            RegisterType(".st", l => new Story(l), true);
            RegisterType(".skl", l => new Skeleton(l));
            RegisterType(".ui", l => new UITemplate(l));
            RegisterType(".atr", l => new ActorTemplate(l));
            RegisterType(".adn", l => new AddonTemplate(l));
            RegisterType(".mt", l => new ModelTemplate(l));
            RegisterType(".pos", l => new AnimationPose(l));
            RegisterType(".eqp", l => new EquipmentTemplate(l));
            RegisterType(".anm", l => new ModelAnimation(l));
            RegisterType(".obj", l => new ModelObject(l));
            RegisterType(".mtl", l => new MaterialLibrary(l));
            RegisterType(".cam", l => new Cam(l));
            RegisterType(".int", l => new InteractionTemplate(l));
            RegisterType(".mvm", l => new MovementTemplate(l));

            // Resources
            RegisterType(".txt", l => l.Read());
            RegisterType(".py", l => Game.Engine.Execute(l.Read()), true);
            RegisterType(".wav", l => new SoundPlayer(l.Open()));
        }

        public static string GetID(string filename)
        {
            int left = filename.IndexOf('[');
            int right = filename.IndexOf(']');
            if (left < 0 || right < 0)
                return null;
            return filename.Substring(left + 1, right - left - 1);
        }

        public string ID { get; }

        public string FullPath { get; }
        public string DirectoryPath { get; }
        public string Extension { get; }

        public bool LoadImmediately { get; } = false;

        public Constructor Constructor { get; }

        object resource;

        public Loader(string path)
        {
            FullPath = path.Replace('\\', '/');
            DirectoryPath = FullPath.Substring(0, FullPath.LastIndexOf('/') + 1);
            Extension = Path.GetExtension(path);

            ID = GetID(Path.GetFileNameWithoutExtension(path));

            if (constructors.ContainsKey(Extension))
                Constructor = constructors[Extension];

            if (immediateLoadingExtensions.Contains(Extension))
                LoadImmediately = true;
        }

        public object Load()
        {
            if (resource == null)
                resource = Constructor(this);
            return resource;
        }

        public T Load<T>()
        {
            return (T)Load();
        }

        protected abstract Stream Open();

        public string Read()
        {
            using (StreamReader r = new StreamReader(Open()))
            {
                return r.ReadToEnd();
            }
        }
        
        public Loader Find(string filename)
        {
            Loader l;
            // Try relative
            if ((l = Game.GetResource(DirectoryPath + filename)) != null)
                return l;
            // Try absolute
            if ((l = Game.GetResource(filename)) != null)
                return l;
            // Use id if all failed
            l = Game.GetResource(Path.GetFileNameWithoutExtension(filename));
            if (l.ID + l.Extension == filename)
                return l;
            return null;
        }

        public T Resource<T>(string filename)
        {
            return Find(filename).Load<T>();
        }

        public List<Instruction> Parse()
        {
            List<Instruction> ins = new List<Instruction>();

            using (StreamReader reader = new StreamReader(Open()))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // Ignore comments
                    int index = line.IndexOf('#');
                    if (index >= 0)
                        line = line.Substring(0, index).Trim();

                    // Ignore empty line
                    if (line == "")
                        continue;

                    ins.Add(new Instruction(line));
                }
            }

            return ins;
        }
        
        public override string ToString() => $"ResourceLoader \"{FullPath}\"";
    }

    public class FileLoader : Loader
    {
        public string DataPath { get; }

        public FileLoader(string data, string path) : base(path)
        {
            DataPath = data;
        }

        protected override Stream Open()
        {
            return File.OpenRead(Path.Combine(DataPath, FullPath));
        }
    }

    public class ZipLoader : Loader
    {
        public ZipArchiveEntry Entry { get; }

        public ZipLoader(ZipArchiveEntry entry) : base(entry.FullName)
        {
            Entry = entry;
        }

        protected override Stream Open()
        {
            return Entry.Open();
        }
    }
}

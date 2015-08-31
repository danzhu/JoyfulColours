using IronPython.Hosting;
using JoyfulColours.Animations;
using JoyfulColours.Elements;
using JoyfulColours.Interface;
using JoyfulColours.Library;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace JoyfulColours
{
    public static class Game
    {
        static ScriptEngine engine;
        public static ScriptEngine Engine => engine;

        static Viewport3D viewport;
        public static Viewport3D Viewport => viewport;

        static Cam camera;
        public static Cam Camera
        {
            get { return camera; }
            set
            {
                camera = value;
                Viewport.Camera = value.Camera;
            }
        }

        // TODO: Multi-scene
        static Scene scene;
        public static Scene Scene
        {
            get { return scene; }
            set
            {
                if (scene != null)
                    viewport.Children.Remove(scene);
                scene = value;
                if (scene != null)
                    viewport.Children.Add(scene);
            }
        }

        static HashSet<Loader> queuedLoaders
            = new HashSet<Loader>();

        static Dictionary<string, Loader> loaders
            = new Dictionary<string, Loader>();

        static HashSet<string> overrides = new HashSet<string>();

        public static bool WarnConflicts { get; set; }
        
        public static void Initialize(MainWindow window)
        {
            viewport = window.viewport;

            // Initialize script engine
            engine = Python.CreateEngine();
            ScriptRuntime runtime = engine.Runtime;
            runtime.LoadAssembly(typeof(Game).Assembly);
            runtime.LoadAssembly(typeof(UIElement).Assembly);
            runtime.LoadAssembly(typeof(FrameworkElement).Assembly);

            // Initialize all modules
            Animation.Initialize();
            Cinema.Initialize(window);
            Easings.Initialize();
            Loader.Initialize();

            Load();
        }

        public static void Reload()
        {
            viewport.Children.Clear();
            loaders.Clear();

            Load();
        }

        public const string Loaded = "loaded";

        public static void Load()
        {
            // TODO: Make loading more flexible
            LoadLooseFiles("data");
            LoadQueued();
            Event.Raise(typeof(Game), Loaded);
        }

        public static void LoadArchive(string path)
        {
            ZipArchive archive = new ZipArchive(File.OpenRead(path));
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                Stream stream = entry.Open();
                string filepath = entry.FullName;
                ZipLoader l = new ZipLoader(entry);
                AddResourceLoader(l);
            }
        }

        public static void LoadLooseFiles(string path)
        {
            // Create loaders for every object
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                Stream stream = File.OpenRead(file.FullName);
                string filepath = file.FullName.Substring(dir.FullName.Length + 1);
                Loader l = new FileLoader(path, filepath);
                AddResourceLoader(l);
            }
        }

        public static void AddResourceLoader(Loader l)
        {
            CheckConflict(l);
            loaders[l.FullPath] = l;
            if (l.ID != null)
                loaders[l.ID] = l;
            if (l.LoadImmediately)
                queuedLoaders.Add(l);
        }

        static void CheckConflict(Loader l)
        {
            if (!WarnConflicts)
                return;
            // Check full path
            if (overrides.Contains(l.FullPath))
                Cinema.Notify($"Possible path conflict: {l.FullPath}");
            else
                overrides.Add(l.FullPath);
            // Check id if applicable
            if (l.ID == null)
                return;
            if (overrides.Contains(l.ID))
                Cinema.Notify($"Possible ID conflict: {l.ID}");
            else
                overrides.Add(l.ID);
        }

        /// <summary>
        /// Actively load each object in game.
        /// Already loaded objects will be ignored.
        /// <para>
        /// Note: Passive laoding is also possible.
        /// Just don't call this method and start getting objects directly.
        /// Objects will be loaded upon being acquired.
        /// </para>
        /// </summary>
        public static void LoadObjects()
        {
            foreach (Loader l in loaders.Values)
                if (l.Constructor != null)
                    l.Load();
        }

        public static void LoadQueued()
        {
            foreach (Loader l in queuedLoaders)
                l.Load();
            queuedLoaders.Clear();
        }

        internal static Loader GetResource(string name)
        {
            if (!loaders.ContainsKey(name))
                return null;
            return loaders[name];
        }

        public static T Get<T>(string name)
        {
            return (T)loaders[name].Load();
        }

        public static object Get(string name)
        {
            return loaders[name].Load();
        }
    }
}

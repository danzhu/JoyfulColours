from JoyfulColours import *
from JoyfulColours.Animations import *
from JoyfulColours.Interface import *
from JoyfulColours.Procedures import *

def Text(name):
    return Game.Get("texts/intro/" + name + ".txt")
    return name

scene = Game.Get('classroom')
cam = Game.Get('screen')
surf = scene['surf'].UIs['Display'].Script

#ui = UI(Game.Get('dialogs'))
#Dialog = ui.Script.Dialog

def Start(sender, e):
    Game.Scene = scene
    Game.Camera = cam
    #Cinema.AddUI(ui)
    
def Complete(sender, e):
    Cinema.Restore(1).Start()

Game.Get('intro').Link(Sequence([
    Concurrence({
        surf.Type(Text('title')),
        Sequence([
            Cinema.Subtitle(line, len(line) / 30 + 2) for line in Text('intro').splitlines()
            ])
        }),
    surf.Print(Text('ascii')),
    Animation(1),
    cam.Animations['back'],
    #Dialog('Hi', 'Hello', '123', '456', None),
    Animation(0.5),
    Cinema.Fade(2),
    ]), Start, Complete)

from JoyfulColours import *
from JoyfulColours.Animations import *
from JoyfulColours.Elements import *
from JoyfulColours.Procedures import *
from JoyfulColours.Logic import *

scene = Game.Get('test')
cam = Game.Get('def')
someone = Actor(Game.Get('someone'))
someone.Equip(Equipment(Game.Get('female_hair01')))
someone.Equip(Equipment(Game.Get('shirt')))
someone.Equip(Equipment(Game.Get('trousers')))
loop = Loop(Game.Get('walk').Create(someone))

def Start(sender, e):
    scene.Load()
    Game.Camera = cam
    Game.Viewport.Children.Add(someone)

def Complete(sender, e):
    scene.Unload()
    Game.Viewport.Children.Remove(someone)

Game.Get('nothing').Link(Sequence([
    Concurrence({
        loop,
        Sequence([
            Animation(20),
            Execution(loop.Stop)
            ])
        }),
    ]), Start, Complete)

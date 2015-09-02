from JoyfulColours import *
from JoyfulColours.Elements import *
from JoyfulColours.Library import *

actor = Actor(Game.Get('someone'))
actor.Position = Position3D(4, 1, 3)
actor.Direction = Direction(2)
Game.Viewport.Children.Add(actor)

actor
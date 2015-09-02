from JoyfulColours import *
from JoyfulColours.Animations import *
from JoyfulColours.Logic import *

player = Game.Get('player')
cam = Game.Get('def')

forward = Movement(Game.Get('forward'), player)
backward = Movement(Game.Get('backward'), player)
left = Movement(Game.Get('left'), player)
right = Movement(Game.Get('right'), player)

Game.Scene = Game.Get('classroom')
Game.Camera = cam

def Start(sender, e):
    cam.Follow(player)
    # Game.Camera.Camera.Transform = player.Translation

control = Game.Get('movement')
control.RegisterKey('W', forward)
control.RegisterKey('S', backward)
control.RegisterKey('A', left)
control.RegisterKey('D', right)
Event.Register(control, 'started', Start)

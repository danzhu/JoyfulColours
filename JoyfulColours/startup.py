if False:
    import clr
    clr.AddReference('JoyfulColours')
    clr.AddReference('PresentationCore')
    clr.AddReference('PresentationFramework')
    from __builtin__ import *
from JoyfulColours import *
from JoyfulColours.Procedures import *

Game.LoadLooseFiles('data')
#Game.LoadObjects()
Game.LoadQueued()

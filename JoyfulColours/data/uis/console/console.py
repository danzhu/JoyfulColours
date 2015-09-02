from JoyfulColours import *
from JoyfulColours.Animations import *
from JoyfulColours.Procedures import *
from JoyfulColours.Logic import *
from System.Windows.Controls import TextBlock

class TypeAnimation(Animation):
    def __new__(cls, t, cps = 10):
        return Animation.__new__(cls)
    def __init__(self, t, cps = 10):
        self.text = t
        self.length = len(t)
        self.cps = cps
        self.output = TextBlock()
        console.Children.Add(self.output)
        self.Updated += self.UpdateText
        self.Skipped += self.SkipTyping

    def UpdateText(self, sender, e):
        i = int((self.Time - self.StartTime) * self.cps)
        self.output.Text = self.text[:i]
        if i >= self.length:
            self.Complete()

    def SkipTyping(self, sender, e):
        self.output.Text = self.text

def Print(t):
    return Event[str](PrintText, t)

def PrintText(t):
    tb = TextBlock()
    tb.Text = t
    console.Children.Add(tb)

console = Game.Get('console')
def New(sender, e):
    script = e.Args[0].Script
    script.Type = TypeAnimation
    script.Print = Print
Event.Register(console, 'new', New)
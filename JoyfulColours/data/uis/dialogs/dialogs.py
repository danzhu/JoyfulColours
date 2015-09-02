from JoyfulColours import *
from JoyfulColours.Procedures import *
from System.Windows import Visibility

class Dialog(Choice):
    def __new__(cls, title, content, ok, cancel, cons):
        return Choice.__new__(cls, cons)
    def __init__(self, title, content, ok, cancel, cons):
        self.title = title
        self.content = content
        self.ok = ok
        self.cancel = cancel
    def OnStarted(self, e):
        dialog.Visibility = Visibility.Visible
        title.Text = self.title
        content.Text = self.content
        ok.Content = self.ok
        cancel.Content = self.cancel
        ok.Click += self.OK
        cancel.Click += self.Cancel
        Choice.OnStarted(self, e)
    def OnCompleted(self, e):
        dialog.Visibility = Visibility.Collapsed
        Choice.OnCompleted(self, e)
    def OK(self, sender, e):
        self.Complete(0)
    def Cancel(self, sender, e):
        self.Complete(1)
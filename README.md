# Reliable Mecanim Events
Reliable Mecanim Events is intended for replacing the existing Animation Events in Unity.

Events placed directly in clips are replaced by event handler on animator states.
Inspired by Firewatch's animator events.

**WARNING: this repository isn't used for development, it's a mirror of changes done on Plastic SCM server of the game I'm using it for.
Not guaranteed to be bug free.**

###Mecanim Event
![Mecanim Event inspector](https://github.com/beatrate/ReliableMecanimEvents/blob/master/Images/reliable-mecanim-event-inspector.PNG)

Each Mecanim Event is a handler for all possible events raised by the animator states. It contains animator event instances raised on state entry, state exit as well as events raised during state update.

**Dispatch Mode:** determines whether event instance is dispatched using MonoBehaviour's SendMessage() or raised on Mecanime Event Receiver on the animator.
**Method Name:** event name.
**Normalized Time:** normalized animation time of the event.
**Always Trigger:** changes whether an event instance has to be raised if the state exited before event's normalized time.
**Repeat On Loop:** should the event be repeated each animation loop.
**Parameter:** Can be of type None, Float, Int, Bool, EventInstance. Type ignored and forced to Event Instance when dispatching using an Event Receiver.
**Animator Actions:** Allow changing animator on event dispatch.
**Debug Break:** Triggers editor debug break on event dispatch.
**Debug Log:** Log to a console on event dispatch.
**Debug String:** Optional message to log.

###Mecanim Event Receiver
A simple component added to the animator that can be used for animator event dispatch to avoid slow reflection from SendMessage calls.
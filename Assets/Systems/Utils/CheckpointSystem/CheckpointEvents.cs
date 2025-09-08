using EventBus;

namespace Checkpoint
{
    /// <summary>
    /// This Event will fire everytime the player reaches a new checkpoint.
    /// </summary>
    public struct CheckpointReached : IEvent
    {

    }
    /// <summary>
    /// This Event will fire when the world needs to be reset, usually because the 
    /// player is about to respawn.
    /// </summary>
    public struct ResetWorld : IEvent
    {

    }
}
namespace Spawning
{
    public interface ISpawnable<Id>
    {
        public void Init(ObjectData<Id> data);
    }
    public interface ISpawner<Id, T> where T : ISpawnable<Id>
    {
        /// <summary>
        /// Create a new object from an ObjectData instance.
        /// </summary>
        /// <param name="data">The data which will be used to create a new object.</param>
        /// <returns>The new object that was created.</returns>
        public T Create(ObjectData<Id> data);
    }
}
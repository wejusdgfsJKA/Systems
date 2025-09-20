namespace Spawning.Pooling
{
    public abstract class Manager : Spawner
    {
        public abstract void ReturnToPool(Poolable poolable);
        public override Spawnable Obtain(Spawnable spawnable)
        {
            var s = GetFromPool((Poolable)spawnable);
            if (s != null)
            {
                s.ResetObject();
            }
            else
            {
                s = (Poolable)base.Obtain(spawnable);
                s.Manager = this;
            }
            return s;
        }
        public abstract Poolable GetFromPool(Poolable poolable);
    }
}
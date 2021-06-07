namespace ParallelTasks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using VRage.Network;

    internal class FutureWork<T> : AbstractWork
    {
        public override void DoWork(WorkData workData = null)
        {
            this.Result = this.Function();
        }

        public static FutureWork<T> GetInstance() => 
            Singleton<Pool<FutureWork<T>>>.Instance.Get(Thread.get_CurrentThread());

        public void ReturnToPool()
        {
            if (this.ID < 0x7fffffff)
            {
                int iD = this.ID;
                this.ID = iD + 1;
                this.Function = null;
                this.Result = default(T);
                Singleton<Pool<FutureWork<T>>>.Instance.Return(Thread.get_CurrentThread(), (FutureWork<T>) this);
            }
        }

        public int ID { get; private set; }

        public Func<T> Function { get; set; }

        public T Result { get; set; }

        private class ParallelTasks_FutureWork`1<>Actor : IActivator, IActivator<FutureWork<T>>
        {
            private sealed FutureWork<T> CreateInstance() => 
                new FutureWork<T>();

            private sealed object CreateInstance() => 
                new FutureWork<T>();
        }
    }
}


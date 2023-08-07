namespace PPlus.Tests.Util
{
    public abstract class BaseTest : IDisposable
    {
        private static readonly AutoResetEvent waitHandle;
        static BaseTest()
        {
            waitHandle = new(true);
        }

        public Action? ActionOnDispose { get; set; } = null;

        protected BaseTest()
        {
            ActionOnDispose = null;
            waitHandle.WaitOne();
            Environment.SetEnvironmentVariable("PromptPlusOverUnitTest", "true");
            PromptPlus.Setup();
            PromptPlus.Reset();
            PromptPlus.Clear();
        }

        public void Dispose()
        {
            ActionOnDispose?.Invoke();
            Environment.SetEnvironmentVariable("PromptPlusOverUnitTest", null);
            waitHandle.Set();
        }

        public void CompletesIn(int timeout, Action action, bool skipexception = false)
        {
            var task = Task.Factory.StartNew(action);
            var completedInTime = task.Wait(TimeSpan.FromMilliseconds(timeout));

            if (task.Exception != null)
            {
                if (skipexception)
                {
                    return;
                }
                if (task.Exception.InnerExceptions.Count == 1)
                {
                    throw task.Exception.InnerExceptions[0];
                }
                throw task.Exception;
            }

            if (!completedInTime)
            {
                throw new TimeoutException($"Task did not complete in {timeout} seconds.");
            }
        }
    }
}

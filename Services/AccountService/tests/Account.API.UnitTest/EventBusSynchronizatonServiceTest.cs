using System;
using System.Collections.Generic;
using System.Text;
using Account.API.Services;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Account.API.UnitTest
{
    public class EventBusSynchronizatonServiceTest
    {
        private readonly IEventBusSynchronizationService eventBusSynchronizationService;

        public EventBusSynchronizatonServiceTest()
        {
            this.eventBusSynchronizationService = new EventBusSynchronizationService();
        }


        [Fact]
        public async System.Threading.Tasks.Task Test_CheckIFHasSynchronizationFinish()
        {
            var key = Guid.NewGuid();
            this.eventBusSynchronizationService.EventSynchronizationList.Add(key,
                new SynchronizationDetails {Token = new System.Threading.CancellationTokenSource() });

            // Start the loop
            var task = this.eventBusSynchronizationService.CheckIFHasSynchronizationFinish(key);
                      
            await Task.Delay(30);

            // Check if the task is running and waiting for synchronization
            var eventTask = this.eventBusSynchronizationService.EventSynchronizationList.Where(x => x.Key.Equals(key)).FirstOrDefault();
            Assert.NotEqual(default, eventTask.Key);
            Assert.Equal(eventTask.Key, key);

            Assert.False(eventTask.Value.Token.IsCancellationRequested);
            eventTask.Value.Token.Cancel();
            Task.WaitAny(task);

            Assert.True(eventTask.Value.Token.IsCancellationRequested);
        }
    }
}

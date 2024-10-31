using System.Threading.Tasks;
using TaskMaster.Infrastructure.Models;

namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private ApplicationDbContext context;
        private IRepository repository;
        private INotificationService notificationService;
        private Notification not;
        private Notification not2;
        private static string userId = Guid.NewGuid().ToString();

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}

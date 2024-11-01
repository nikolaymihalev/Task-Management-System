namespace TaskMaster.UnitTests
{
    [TestFixture]
    public class StatisticsServiceTests
    {
        private ApplicationDbContext context;
        private IRepository repository;
        private static string userId = Guid.NewGuid().ToString();
        private IStatisticsService statisticsService;
        private Infrastructure.Models.Task task;
        private Infrastructure.Models.Task task2;
        private Infrastructure.Models.Task task3;
        private Infrastructure.Models.Task task4;

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

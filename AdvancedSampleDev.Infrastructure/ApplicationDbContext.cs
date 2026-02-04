using Microsoft.EntityFrameworkCore;

namespace AdvancedSampleDev.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options);


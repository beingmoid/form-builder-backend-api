using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FormBuilder.Model
{

    
    public class FormTemplate
    {
        public FormTemplate()
        {
            this.Templates = new HashSet<Template>();
        }
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public string TemplateName { get; set; }
        public ICollection<Template> Templates { get; set; }
    }
 
    public class Template {
        public Template()
        {
            this.Questions = new HashSet<Questions>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
     //   public int FormTemplateId { get; set; }
        public FormTemplate FormTemplate { get; set; }
        public ICollection<Questions> Questions { get; set; }


    }
    
    public class Questions
    {
        public Questions()
        {
            this.Reponses = new HashSet<TypeOfResponse>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int QuestionId { get; set; }
     //   public int TemplateId { get; set; }
        public Template Template { get; set; }
        public string Question { get; set; }
    
        public ICollection<TypeOfResponse> Reponses { get; set; }
        public bool IsScored { get; set; }
        public int Score { get; set; }

    }
  
    public class TypeOfResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string ResponseText { get; set; }
        public string ResponseType { get; set; }
        public bool IsScored{ get; set; }
        public bool IsMultiResponse { get; set; }
      //  public int QuestionId { get; set; }
        public Questions Questions  { get; set; }
    }
    public class FormBuilderContext:DbContext
    {
        public FormBuilderContext(DbContextOptions options) 
            :base(options)
        {

        }
        public FormBuilderContext()
        {

        }
        public DbSet<FormTemplate> FormTemplates { get; set; }
        public DbSet<Template> Templates    { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<TypeOfResponse> TypeOfResponses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-M5C4BD6;Initial Catalog=dbFormBuilder;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormTemplate>().HasMany<Template>(x=>x.Templates).WithOne(x=>x.FormTemplate);
            modelBuilder.Entity<Template>().HasMany<Questions>(x => x.Questions).WithOne(x => x.Template);
            modelBuilder.Entity<Questions>().HasMany<TypeOfResponse>(x => x.Reponses).WithOne(x => x.Questions);
        }
    }
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FormBuilderContext>
    {
        public FormBuilderContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<FormBuilderContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer("Data Source=DESKTOP-M5C4BD6;Initial Catalog=dbFormBuilder;Integrated Security=True");
            return new FormBuilderContext(builder.Options);
        }
    }
}


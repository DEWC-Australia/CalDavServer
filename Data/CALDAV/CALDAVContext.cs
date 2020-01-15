using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CalDav.Data.CALDAV
{
    public partial class CALDAVContext : DbContext
    {
        public CALDAVContext()
        {
        }

        public CALDAVContext(DbContextOptions<CALDAVContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CalDavServer> CalDavServer { get; set; }
        public virtual DbSet<CalendarEvent> CalendarEvent { get; set; }
        public virtual DbSet<CalendarFile> CalendarFile { get; set; }
        public virtual DbSet<CalendarFolderInfo> CalendarFolderInfo { get; set; }
        public virtual DbSet<CalendarFreeBusy> CalendarFreeBusy { get; set; }
        public virtual DbSet<CalendarJournal> CalendarJournal { get; set; }
        public virtual DbSet<CalendarTimeZone> CalendarTimeZone { get; set; }
        public virtual DbSet<CalendarToDo> CalendarToDo { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<DateTimeRange> DateTimeRange { get; set; }
        public virtual DbSet<FolderInfo> FolderInfo { get; set; }
        public virtual DbSet<FolderType> FolderType { get; set; }
        public virtual DbSet<Frequency> Frequency { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<Recurrence> Recurrence { get; set; }
        public virtual DbSet<Statuses> Statuses { get; set; }
        public virtual DbSet<UserFolderAccess> UserFolderAccess { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<CalDavServer>(entity =>
            {
                entity.HasKey(e => e.SystemId)
                    .HasName("PK__CalDavSe__9394F6AAA32CEAEC");

                entity.ToTable("CalDavServer", "CALDAV");

                entity.Property(e => e.SystemId)
                    .HasColumnName("SystemID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AllowOptions)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.BaseAclPath)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.BaseCalendarHomeSetPath)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PublicOptions)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ContextPathNavigation)
                    .WithMany(p => p.CalDavServer)
                    .HasForeignKey(d => d.ContextPath)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalDavSer__Conte__58B1FFE3");
            });

            modelBuilder.Entity<CalendarEvent>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.CalendarFileId, e.EventId })
                    .HasName("PK__Calendar__580241C4EE6F4FD2");

                entity.ToTable("CalendarEvent", "CALDAV");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.EventId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Transparency).HasMaxLength(255);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.CalendarEvent)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__CalendarE__Class__039C5DE8");

                entity.HasOne(d => d.OrganizerNavigation)
                    .WithMany(p => p.CalendarEvent)
                    .HasForeignKey(d => d.Organizer)
                    .HasConstraintName("FK__CalendarE__Organ__04908221");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CalendarEvent)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__CalendarE__Statu__0584A65A");

                entity.HasOne(d => d.CalendarFile)
                    .WithMany(p => p.CalendarEvent)
                    .HasForeignKey(d => new { d.FolderId, d.CalendarFileId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarEvent__00BFF13D");
            });

            modelBuilder.Entity<CalendarFile>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.CalendarFileId })
                    .HasName("PK__Calendar__497B050C84BD8A45");

                entity.ToTable("CalendarFile", "CALDAV");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.CalendarFileId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Etag)
                    .IsRequired()
                    .HasColumnName("ETag")
                    .IsRowVersion();

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ProdId)
                    .HasColumnName("ProdID")
                    .HasMaxLength(100);

                entity.Property(e => e.Scale).HasMaxLength(100);

                entity.Property(e => e.Version).HasMaxLength(50);

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.CalendarFile)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarF__Folde__6517D6C8");

                entity.HasOne(d => d.FolderNavigation)
                    .WithMany(p => p.CalendarFile)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarF__Folde__6423B28F");
            });

            modelBuilder.Entity<CalendarFolderInfo>(entity =>
            {
                entity.HasKey(e => e.FolderId)
                    .HasName("PK__Calendar__ACD7109FF6D72026");

                entity.ToTable("CalendarFolderInfo", "CALDAV");

                entity.Property(e => e.FolderId)
                    .HasColumnName("FolderID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CalendarColor).HasMaxLength(200);

                entity.Property(e => e.CalendarDescription).HasMaxLength(200);

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('text/calendar; component=vevent')");

                entity.Property(e => e.Ctag)
                    .IsRequired()
                    .HasColumnName("CTag")
                    .IsRowVersion();

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Folder)
                    .WithOne(p => p.CalendarFolderInfo)
                    .HasForeignKey<CalendarFolderInfo>(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarF__Folde__54E16EFF");
            });

            modelBuilder.Entity<CalendarFreeBusy>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.CalendarFileId, e.FreeBusyId })
                    .HasName("PK__Calendar__3541A19B8F0EE36D");

                entity.ToTable("CalendarFreeBusy", "CALDAV");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.FreeBusyId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.End)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Start)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.OrganizerNavigation)
                    .WithMany(p => p.CalendarFreeBusy)
                    .HasForeignKey(d => d.Organizer)
                    .HasConstraintName("FK__CalendarF__Organ__20389C96");

                entity.HasOne(d => d.CalendarFile)
                    .WithMany(p => p.CalendarFreeBusy)
                    .HasForeignKey(d => new { d.FolderId, d.CalendarFileId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarFreeBusy__1C680BB2");
            });

            modelBuilder.Entity<CalendarJournal>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.CalendarFileId, e.JournalId })
                    .HasName("PK__Calendar__AE5E040F8F89DD20");

                entity.ToTable("CalendarJournal", "CALDAV");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.JournalId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.CalendarJournal)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__CalendarJ__Class__18977ACE");

                entity.HasOne(d => d.OrganizerNavigation)
                    .WithMany(p => p.CalendarJournal)
                    .HasForeignKey(d => d.Organizer)
                    .HasConstraintName("FK__CalendarJ__Organ__16AF325C");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CalendarJournal)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__CalendarJ__Statu__17A35695");

                entity.HasOne(d => d.CalendarFile)
                    .WithMany(p => p.CalendarJournal)
                    .HasForeignKey(d => new { d.FolderId, d.CalendarFileId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarJournal__14C6E9EA");
            });

            modelBuilder.Entity<CalendarTimeZone>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.CalendarFileId, e.TimeZoneId })
                    .HasName("PK__Calendar__6703D68B9060F124");

                entity.ToTable("CalendarTimeZone", "CALDAV");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.TimeZoneId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.CalendarFile)
                    .WithMany(p => p.CalendarTimeZone)
                    .HasForeignKey(d => new { d.FolderId, d.CalendarFileId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarTimeZone__0955373E");
            });

            modelBuilder.Entity<CalendarToDo>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.CalendarFileId, e.ToDoId })
                    .HasName("PK__Calendar__485AD581A0CC260E");

                entity.ToTable("CalendarToDo", "CALDAV");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.ToDoId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.Completed)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.CalendarToDo)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__CalendarT__Class__10F65906");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CalendarToDo)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__CalendarT__Statu__100234CD");

                entity.HasOne(d => d.CalendarFile)
                    .WithMany(p => p.CalendarToDo)
                    .HasForeignKey(d => new { d.FolderId, d.CalendarFileId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CalendarToDo__0D25C822");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "CALDAV");

                entity.HasIndex(e => e.Value)
                    .HasName("UQ__Category__07D9BBC263C3B08D")
                    .IsUnique();

                entity.Property(e => e.CategoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class", "CALDAV");

                entity.Property(e => e.ClassId).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact", "CALDAV");

                entity.Property(e => e.ContactId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Directory)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SentBy)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<DateTimeRange>(entity =>
            {
                entity.ToTable("DateTimeRange", "CALDAV");

                entity.Property(e => e.DateTimeRangeId)
                    .HasColumnName("DateTimeRangeID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.To).HasColumnName("TO");
            });

            modelBuilder.Entity<FolderInfo>(entity =>
            {
                entity.HasKey(e => e.FolderId)
                    .HasName("PK__FolderIn__ACD7109F3494B630");

                entity.ToTable("FolderInfo", "CALDAV");

                entity.HasIndex(e => e.Path)
                    .HasName("UQ__FolderIn__A15FA6CB0ADC86F9")
                    .IsUnique();

                entity.Property(e => e.FolderId)
                    .HasColumnName("FolderID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ParentFolderId).HasColumnName("ParentFolderID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.FolderTypeNavigation)
                    .WithMany(p => p.FolderInfo)
                    .HasForeignKey(d => d.FolderType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FolderInf__Folde__4F2895A9");

                entity.HasOne(d => d.ParentFolder)
                    .WithMany(p => p.InverseParentFolder)
                    .HasForeignKey(d => d.ParentFolderId)
                    .HasConstraintName("FK__FolderInf__Paren__4E347170");
            });

            modelBuilder.Entity<FolderType>(entity =>
            {
                entity.HasKey(e => e.FolderType1)
                    .HasName("PK__FolderTy__F913F570A94A115E");

                entity.ToTable("FolderType", "CALDAV");

                entity.Property(e => e.FolderType1)
                    .HasColumnName("FolderType")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Frequency>(entity =>
            {
                entity.ToTable("Frequency", "CALDAV");

                entity.Property(e => e.FrequencyId)
                    .HasColumnName("FrequencyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property", "CALDAV");

                entity.Property(e => e.PropertyId)
                    .HasColumnName("PropertyID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Parameters).IsRequired();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Recurrence>(entity =>
            {
                entity.ToTable("Recurrence", "CALDAV");

                entity.Property(e => e.RecurrenceId)
                    .HasColumnName("RecurrenceID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ByDay).HasMaxLength(50);

                entity.Property(e => e.BySetPos).HasMaxLength(50);

                entity.Property(e => e.FrequencyId).HasColumnName("FrequencyID");

                entity.Property(e => e.WeekStart).HasMaxLength(50);

                entity.HasOne(d => d.Frequency)
                    .WithMany(p => p.Recurrence)
                    .HasForeignKey(d => d.FrequencyId)
                    .HasConstraintName("FK__Recurrenc__Frequ__6CB8F890");
            });

            modelBuilder.Entity<Statuses>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__Statuses__C8EE2043A03D4141");

                entity.ToTable("Statuses", "CALDAV");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<UserFolderAccess>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FolderId })
                    .HasName("PK__UserFold__FD45BDA511E6EBAE");

                entity.ToTable("UserFolderAccess", "CALDAV");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.UserFolderAccess)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserFolde__Folde__605321AB");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFolderAccess)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserFolde__UserI__5F5EFD72");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserProf__1788CCAC379811C0");

                entity.ToTable("UserProfile", "CALDAV");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.AclFolderNavigation)
                    .WithMany(p => p.UserProfileAclFolderNavigation)
                    .HasForeignKey(d => d.AclFolder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserProfi__AclFo__5C8290C7");

                entity.HasOne(d => d.CalendarHomeSetNavigation)
                    .WithMany(p => p.UserProfileCalendarHomeSetNavigation)
                    .HasForeignKey(d => d.CalendarHomeSet)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserProfi__Calen__5B8E6C8E");
            });
        }
    }
}

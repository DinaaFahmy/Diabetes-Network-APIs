using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication.Models
{
    public partial class DiabetesSystem2Context : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public DiabetesSystem2Context()
        {
        }

        public DiabetesSystem2Context(DbContextOptions<DiabetesSystem2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Certificates> Certificates { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<ChecksUps> ChecksUps { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<DrugPatient> DrugPatient { get; set; }
        public virtual DbSet<Drugs> Drugs { get; set; }
        public virtual DbSet<Msg> Msg { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationAnswer> NotificationAnswer { get; set; }
        public virtual DbSet<NotificationAsked> NotificationAsked { get; set; }
        public virtual DbSet<NotificationFollow> NotificationFollow { get; set; }
        public virtual DbSet<NotificationMedicalInfo> NotificationMedicalInfo { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientDoctorsFollow> PatientDoctorsFollow { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<QuestionDoctorsMention> QuestionDoctorsMention { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Reactions> Reactions { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<UserSavedPosts> UserSavedPosts { get; set; }
        public virtual DbSet<UserSavedQuestion> UserSavedQuestion { get; set; }
        public virtual DbSet<Users> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=DiabetesSystem2;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Answers>(entity =>
            {
                entity.HasKey(e => e.AnswerId);

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Answers_Questions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Answers_Users");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Certificates>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Certificate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.University)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Certificates)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Certificates_Doctor");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ChatId });

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ChatId)
                    .HasColumnName("ChatID")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.ChatNavigation)
                    .WithMany(p => p.Chat)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Msg");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Chat)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Users");
            });

            modelBuilder.Entity<ChecksUps>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasMaxLength(100);

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.ChecksUps)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_ChecksUps_Patient1");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.DoctorId)
                    .HasColumnName("DoctorID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.HasOne(d => d.DoctorNavigation)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doctor_Users");
            });

            modelBuilder.Entity<DrugPatient>(entity =>
            {
                entity.HasKey(e => new { e.DrugId, e.PatientId });

                entity.ToTable("Drug_Patient");

                entity.Property(e => e.DrugId).HasColumnName("DrugID");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.DrugPatient)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drug_Patient_Drugs");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.DrugPatient)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drug_Patient_Patient1");
            });

            modelBuilder.Entity<Drugs>(entity =>
            {
                entity.HasKey(e => e.DrugId);

                entity.Property(e => e.DrugId).HasColumnName("DrugID");

                entity.Property(e => e.DosageType)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DrugName).HasMaxLength(50);

                entity.Property(e => e.ImageSource).HasMaxLength(50);
            });

            modelBuilder.Entity<Msg>(entity =>
            {
                entity.Property(e => e.MsgId).HasColumnName("MsgID");

                entity.Property(e => e.ChatId).HasColumnName("ChatID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Msg)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Msg_Users");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.NotificationContent).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<NotificationAnswer>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK_Notification_Answer_1");

                entity.ToTable("Notification_Answer");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("NotificationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.NotificationAnswer)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Answer_Answers");

                entity.HasOne(d => d.Notification)
                    .WithOne(p => p.NotificationAnswer)
                    .HasForeignKey<NotificationAnswer>(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Answer_Notification");
            });

            modelBuilder.Entity<NotificationAsked>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK_Notification_Asked_1");

                entity.ToTable("Notification_Asked");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("NotificationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.MentionId).HasColumnName("MentionID");

                entity.HasOne(d => d.Mention)
                    .WithMany(p => p.NotificationAsked)
                    .HasForeignKey(d => d.MentionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Asked_Question_Doctors(Mention)");

                entity.HasOne(d => d.Notification)
                    .WithOne(p => p.NotificationAsked)
                    .HasForeignKey<NotificationAsked>(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Asked_Notification");
            });

            modelBuilder.Entity<NotificationFollow>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK_Notification_Follow_1");

                entity.ToTable("Notification_Follow");

                entity.HasComment("notify Doctor when a patient follows him");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("NotificationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FollowId).HasColumnName("FollowID");

                entity.HasOne(d => d.Notification)
                    .WithOne(p => p.NotificationFollow)
                    .HasForeignKey<NotificationFollow>(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_Follow_Notification");
            });

            modelBuilder.Entity<NotificationMedicalInfo>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK_Notification_MedicalInfo_1");

                entity.ToTable("Notification_MedicalInfo");

                entity.HasComment("Notify Patient that a doctor requested Medicalinfo");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("NotificationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.MedicalInfoId).HasColumnName("MedicalInfoID");

                entity.HasOne(d => d.MedicalInfo)
                    .WithMany(p => p.NotificationMedicalInfo)
                    .HasForeignKey(d => d.MedicalInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_MedicalInfo_Patient_Doctors(follow)");

                entity.HasOne(d => d.Notification)
                    .WithOne(p => p.NotificationMedicalInfo)
                    .HasForeignKey<NotificationMedicalInfo>(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_MedicalInfo_Notification");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId)
                    .HasColumnName("PatientID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Height).HasColumnName("height");

                entity.HasOne(d => d.PatientNavigation)
                    .WithOne(p => p.Patient)
                    .HasForeignKey<Patient>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Users");
            });

            modelBuilder.Entity<PatientDoctorsFollow>(entity =>
            {
                entity.ToTable("Patient_Doctors(follow)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccessMedicalInfo).HasColumnName("Access_MedicalInfo");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.PatienId).HasColumnName("PatienID");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.PatientDoctorsFollow)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK_Patient_Doctors_Doctor");

                entity.HasOne(d => d.Patien)
                    .WithMany(p => p.PatientDoctorsFollow)
                    .HasForeignKey(d => d.PatienId)
                    .HasConstraintName("FK_Patient_Doctors_Patient");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.Property(e => e.PostId)
                    .HasColumnName("PostID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ImageSource).HasMaxLength(50);

                entity.Property(e => e.PostDate).HasColumnType("datetime");

                entity.Property(e => e.ReactionId).HasColumnName("ReactionID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Category");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.Posts)
                    .HasForeignKey<Posts>(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Users");

                entity.HasOne(d => d.Reaction)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ReactionId)
                    .HasConstraintName("FK_Posts_Reactions");
            });

            modelBuilder.Entity<QuestionDoctorsMention>(entity =>
            {
                entity.ToTable("Question_Doctors(Mention)");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Status).HasComment("IF the questions is answered or not");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.QuestionDoctorsMention)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Doctors_Doctor");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionDoctorsMention)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Doctors_Questions");
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.HasKey(e => e.QuestionId);

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Question).HasMaxLength(300);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_Users");
            });

            modelBuilder.Entity<Reactions>(entity =>
            {
                entity.HasKey(e => e.ReactionId);

                entity.Property(e => e.ReactionId).HasColumnName("ReactionID");

                entity.Property(e => e.ReactionName).HasMaxLength(50);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Result)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Test_Patient");
            });

            modelBuilder.Entity<UserSavedPosts>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PostId });

                entity.ToTable("User_SavedPosts");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserSavedPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_SavedPosts_Posts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSavedPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_SavedPosts_Users");
            });

            modelBuilder.Entity<UserSavedQuestion>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.UserId });

                entity.ToTable("User_SavedQuestion");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.UserSavedQuestion)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_SavedQuestion_Questions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSavedQuestion)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_SavedQuestion_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                ////entity.Property(e => e.Email)
                ////    .IsRequired()
                ////    .HasMaxLength(50);

                //entity.Property(e => e.Password).HasMaxLength(50);

                //entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

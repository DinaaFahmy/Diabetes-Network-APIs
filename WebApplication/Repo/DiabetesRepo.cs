using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.ModelViews;
using static WebApplication.Repo.IDiabetes;

namespace WebApplication.Repo {
    public partial class DiabetesRepo : IDiabetes {
        DiabetesSystem2Context db;

        public DiabetesRepo (DiabetesSystem2Context _db) {
            db = _db;
        }

        public Posts AddPost (Posts p) {
            return db.Posts.FromSqlRaw ("AddPost {0}, {1},{2},{3},{4}", p.UserId, p.CategoryId, p.ImageSource, p.PostContent, p.ReactionId).AsEnumerable ().FirstOrDefault ();
        }

        public List<Test> GetAllTests (int patientid, short type) {
            return db.Test.FromSqlRaw ("SelectTestsByType {0},{1} ", patientid, type).ToList ();

        }

        public DrugPatient AddDrug (int drugid, int patientid, string note, int dose) {
            return db.DrugPatient.FromSqlRaw ("AddDrugForPatient {0},{1},{2},{3}", drugid, patientid, note, dose).AsEnumerable ().FirstOrDefault ();
        }

        public ChecksUps UpdateCheckup (ChecksUps checkup) { //////////////////need change
            // db.Database.ExecuteSqlRaw("UpdateCheckups {0}", checkup);
            db.ChecksUps.Update (checkup);
            db.SaveChanges ();
            return checkup;
        }

        public List<Questions> GetMyQuestions (int userId) {
            return db.Questions.FromSqlRaw ("Questions_Select {0}", userId).ToList ();
        }

        //public Answers AddAnswer(int userid, int questionid, string answer)
        //{
        //    return db.Answers.FromSqlRaw("AddAnswer {0},{1},{2}", questionid, answer, userid).AsEnumerable().FirstOrDefault();
        //}

        public List<ChecksUps> GetPatientCheckups (int patientid) {
            return db.ChecksUps.FromSqlRaw ("ChecksUps_Select {0}", patientid).ToList ();
        }

        public List<Drugs> GetAllDrugs () {
            return db.Drugs.ToList ();
        }

        public Certificates AddCertificate (int doctorid, string certificate, string university) {
            return db.Certificates.FromSqlRaw ("AddCertificate  {0}, {1} ,{2}  ", certificate, university, doctorid).ToList ().FirstOrDefault ();
        }

        public ChecksUps AddCheckup (ChecksUps checksUp) {
            var x = db.ChecksUps.Add (checksUp);
            db.SaveChanges ();
            return x.Entity;
            // db.ChecksUps.FromSqlRaw("addcheckup  {0}, {1} ,{2}", patientid, checkuptype, notes).ToList();
        }

        public Test AddTest (Test test) //edited to void, may return testID
        {
            var x = db.Test.Add (test).Entity;
            db.SaveChanges ();
            return x;
            // return db.Test.FromSqlRaw("EXEC dbo.AddTest {0},{1},{2},{3},{4}", result, type, patientid, medication,date).AsEnumerable().FirstOrDefault();
            //int test_id = db.Test.OrderByDescending(p => p.Id).FirstOrDefault().Id;
        }

        public List<Category> GetAllCategories () {
            return db.Category.ToList<Category> ();
        }

        public List<Reactions> GetAllReacts () {
            return db.Reactions.ToList ();
        }

        public List<Notification> GetDoctorNOtifications (int doctorid) {
            return db.Notification.FromSqlRaw ("Notification_Doctor_Select  {0}", doctorid).ToList ();
        }

        public List<GetMyDoctors> GetMyDoctors (int? patient_id) {
            var sp = from f in db.PatientDoctorsFollow
            join u in db.users
            on f.DoctorId equals u.UserId
            where f.PatienId == patient_id
            select new GetMyDoctors {
                id = f.Id,
                doctor_id = f.DoctorId,
                access_med_info = f.AccessMedicalInfo,
                patient_id = patient_id,
                DoctorName = u.UserName,
                img = u.ImageSource,
                type = u.Type,
                // identity_id = u.ID
            };
            return sp.ToList ();
        }

        public List<FollowingPatients> GetMyPatients (int doctorid) {
            var sp = from u in db.users
            join p in db.Patient
            on u.UserId equals p.PatientId
            join f in db.PatientDoctorsFollow
            on p.PatientId equals f.PatienId
            where f.DoctorId == doctorid
            select new FollowingPatients {
                userID = u.UserId,
                user_name = u.UserName,
                img = u.ImageSource,
                medical_cond = p.MedicalCondetion,
                birth_date = p.BirthDate,
                gender = p.Gender,
                weight = p.Weight,
                height = p.Height,
                life_style = p.LifeStyle,
                access_med_info = f.AccessMedicalInfo
            };
            return sp.ToList ();
        }

        public List<Notification> GetPatientNotifications (int patientid) {
            return db.Notification.FromSqlRaw ("EXECUTE dbo.Notification_Patient {0}", patientid).ToList ();
        }

        public List<Posts> GetPosts (int CategoryId) {
            return db.Posts.FromSqlRaw ("Posts_Select  {0}", CategoryId).ToList ();
        }

        public List<SavedPosts> GetSavedPosts (int userID) {
            // return db.savedposts.FromSqlRaw($"SavedPosts {userID}").ToList();
            var sp = from p in db.Posts
            join u in db.UserSavedPosts
            on p.PostId equals u.PostId
            where u.UserId == userID
            select new SavedPosts {
                post_id = p.PostId,
                user_id = p.UserId,
                category_id = p.CategoryId,
                img = p.ImageSource,
                content = p.PostContent,
                react_id = p.ReactionId,
                date = p.PostDate
            };
            return sp.ToList ();
        }

        public List<Questions> GetSavedQuestions (int userId) {
            return db.Questions.FromSqlRaw ("SavedQuestions_Select  {0}", userId).ToList ();
        }

        public bool UnfollowDoctor (int patientid, int doctorid) {
            bool flag = false;
            int x = db.Database.ExecuteSqlRaw ("unfollowdoctor {0}, {1}", doctorid, patientid);
            if (x != 0) flag = true;
            return flag;
        }

        public int GetUserID (string ID) {
            return db.users.Where (a => a.ID == ID).SingleOrDefault ().UserId;
        }

        public GetMyDoctors Update_access_medicalInfo (int followID, short status) {
            db.PatientDoctorsFollow.Find (followID).AccessMedicalInfo = status;
            db.SaveChanges ();
            var data = (from f in db.PatientDoctorsFollow join u in db.users on f.DoctorId equals u.UserId where followID == f.Id

                select new GetMyDoctors {
                    id = f.Id,
                        doctor_id = f.DoctorId,
                        access_med_info = f.AccessMedicalInfo,
                        patient_id = f.PatienId,
                        DoctorName = u.UserName,
                        img = u.ImageSource,
                        type = u.Type,
                        //  identity_id = u.ID
                }).First ();

            return data;
        }

        public bool Deletepost (int postID, int userID) {
            bool flag = false;

            var c = db.Posts.Find (postID);
            if (c != null && c.UserId == userID) {
                db.Posts.Remove (c);
                db.SaveChanges ();
                flag = true;
            }

            return flag;
        }

        public bool DeleteAnswer (int AnsID, int userID) {
            bool flag = false;

            var c = db.Answers.Find (AnsID);
            if (c != null && c.UserId == userID) {
                db.Answers.Remove (c);
                db.SaveChanges ();
                flag = true;
            }

            return flag;
        }

        public bool Deletequestion (int QustionID, int userID) {
            bool flag = false;

            var c = db.Questions.Find (QustionID);
            if (c != null && c.UserId == userID) {
                db.Questions.Remove (c);
                db.SaveChanges ();
                flag = true;
            }

            return flag;
        }

        public List<Posts> GetAllPosts () {
            return db.Posts.FromSqlRaw ("getLatestPosts ").ToList ();
        }

        public List<Posts> GetPostsByUserID (int user_id) {
            return db.Posts.FromSqlRaw ("Posts_Select_ByUserID  {0}", user_id).ToList ();
        }
        public List<Posts> GetPosts (int postID, List<int> CategoryId) {

            List<Posts> post_list2 = new List<Posts> ();
            if (postID == -1) {

                var posts = (from p in db.Posts orderby p.PostDate descending where CategoryId.Contains (p.CategoryId) select p).Take (10).ToList ();
                post_list2.AddRange (posts);

            } else {

                var posts = (from p in db.Posts orderby p.PostDate descending where p.PostId < postID && CategoryId.Contains (p.CategoryId) select p).Take (10).ToList ();

                post_list2.AddRange (posts);
            }
            return post_list2;
        }

        public Patient GetPatient (int id) {
            return db.Patient.FirstOrDefault (a => a.PatientId == id);
        }

        public Doctor GetDoctor (int id) {
            return db.Doctor.FirstOrDefault (a => a.DoctorId == id);
        }

        public Users GetUser (int id) {

            return db.users.FirstOrDefault (a => a.UserId == id);
        }

        public List<Questions> GetQuestionsByUser (int id) {
            return db.Questions.ToList ().FindAll (a => a.UserId == id);
        }

        //////////////////////////////////////////////////////////////
        ///
        public List<Questions> getMentionedInQuestions (int drid) {
            List<QuestionModel> doctorquestions = new List<QuestionModel> ();
            var query = from q in db.Questions
            join a in db.QuestionDoctorsMention
            on q.QuestionId equals a.QuestionId
            where a.DoctorId == drid
            select q;
            return query.ToList ();
        }

        public List<Questions> getAnsweredQuestions (int drid) {
            var query = from q in db.Questions
            join a in db.Answers
            on q.QuestionId equals a.QuestionId
            where a.UserId == drid
            select q;
            return query.ToList ();

        }

        public List<Certificates> GetCertificates (int DrID) {
            return db.Certificates.ToList ().FindAll (c => c.DoctorId == DrID);
        }

        public PatientDoctorsFollow request_access_medicalInfo (int drid, int pid, int FollowID) {
            ///0 refused ,1 requested ,2 approved ,3 null
            //  var x = db.PatientDoctorsFollow.Find(FollowID).AccessMedicalInfo
            if (db.PatientDoctorsFollow.Find (FollowID).AccessMedicalInfo == 3 || db.PatientDoctorsFollow.Find (FollowID).AccessMedicalInfo == 0)

            {
                db.PatientDoctorsFollow.Find (FollowID).AccessMedicalInfo = 1;
                db.SaveChanges ();
                return db.PatientDoctorsFollow.Find (FollowID);
            }
            return null;
            ////if (db.PatientDoctorsFollow.Find(FollowID).AccessMedicalInfo == 3 || db.PatientDoctorsFollow.Find(FollowID).AccessMedicalInfo == 0)
            ////{ var y = db.PatientDoctorsFollow.FromSqlRaw("RequestMedicalInfo {0},{1},{2}", drid, pid, FollowID).ToList().FirstOrDefault();
            ////    db.SaveChanges();

            ////    y = db.PatientDoctorsFollow.Where(a=>a.Id==FollowID).FirstOrDefault();

            ////    return y;
            ////}
            ////// return db.PatientDoctorsFollow.Find(FollowID)
            ////return null;
        }

        public List<AnswerModel> GetAnswers (int QuesId) {
            List<Answers> answers = db.Answers.ToList ().FindAll (c => c.QuestionId == QuesId);
            List<AnswerModel> answers1 = new List<AnswerModel> ();
            for (int i = 0; i < answers.Count; i++) {
                AnswerModel answer = new AnswerModel ();
                answer.Answer = answers[i].Answer;
                answer.UserName = GetUser (answers[i].UserId).UserName;
                answer.Date = answers[i].Date;
                answer.userID = answers[i].UserId;
                answer.role = db.users.Where (a => a.UserId == answer.userID).ToList ().FirstOrDefault ().Type;
                answer.id = answers[i].AnswerId;

                answers1.Add (answer);

            }
            return answers1;
        }
        //////////////////////////////////////////////////////////////////

        PatientProfileModel IDiabetes.getAllPatients (int patientid) {
            var sp = from u in db.users
            join p in db.Patient
            on u.UserId equals p.PatientId
            join f in db.PatientDoctorsFollow
            on u.UserId equals f.PatienId
            join c in db.ChecksUps
            on u.UserId equals c.PatientId
            join t in db.Test
            on u.UserId equals t.PatientId
            join d in db.DrugPatient
            on u.UserId equals d.PatientId
            where u.UserId == patientid

            select new PatientProfileModel {
                userID = patientid,
                user_name = u.UserName,
                img = u.ImageSource,
                medical_cond = p.MedicalCondetion,
                birth_date = p.BirthDate,
                gender = p.Gender,
                weight = p.Weight,
                height = p.Height,
                life_style = p.LifeStyle,
                access_med_info = f.AccessMedicalInfo,
                //testt
                testType = t.Type,
                testResult = t.Result,
                testMedicationsState = t.Medication,
                testDate = t.Date,
                //checkups
                checkupData = c.ResultData,
                checkupType = c.CheckupType,
                checkupNotes = c.Notes,
                checkupStatus = c.Status,
                checkupDate = c.Date,
                //medications
                drug = d.Drug,
                drugDosage = d.Dosage,
                drugNote = d.Note,
            };
            return sp.FirstOrDefault ();
        }

        public void updateAddress (int user_id, string address) {
            Doctor dd = GetDoctor (user_id);
            dd.Address = address;
            db.SaveChanges ();
        }

        public void updatephonenumber (string id, string phone) {
            db.Users.FirstOrDefault (a => a.Id == id).PhoneNumber = phone;
            db.SaveChanges ();
        }
        public void Addecertificate (Certificates certificate) {
            db.Certificates.Add (certificate);
            db.SaveChanges ();
        }

        public List<CommentModel> GetComments (int PostID) {

            var comments = from c in db.Comments
            join u in db.users
            on c.UserID equals u.UserId
            where c.PostID == PostID
            select new CommentModel {
                ID = c.ID,
                PostID = PostID,
                Comment = c.Comment,
                UserName = u.UserName,
                Date = c.Date,
                UserID = u.UserId,

            };
            return comments.ToList ();

        }

        public CommentModel AddComment (Comments comment) {
            comment.Date = DateTime.Now;
            var c = db.Comments.Add (comment).Entity;
            db.SaveChanges ();
            return new CommentModel {
                ID = c.ID,
                    PostID = c.PostID,
                    UserName = db.users.Where (a => a.UserId == comment.UserID).FirstOrDefault ().UserName,
                    Comment = c.Comment,
                    UserID = c.UserID,
                    Date = c.Date

            };
        }

        public bool DeleteComment (int CommentID, int userID) {
            bool flag = false;

            var c = db.Comments.Find (CommentID);
            if (c != null && c.UserID == userID) {
                db.Comments.Remove (c);
                db.SaveChanges ();
                flag = true;
            }

            return flag;
        }

        public List<AnswerModel> GetDoctorAnswers (int QuestionID) {
            List<AnswerModel> answers = new List<AnswerModel> ();
            answers = (from A in db.Answers join u in db.users on A.UserId equals u.UserId where u.Type == true && A.QuestionId == QuestionID orderby A.Date descending select new AnswerModel {
                Answer = A.Answer,
                    UserName = u.UserName,
                    Date = A.Date,
                    id = A.AnswerId,
                    UserType = u.Type,
                    userID = A.UserId,
                    role = u.Type,

            }).ToList ();

            return answers;
        }

        public List<AnswerModel> GetPatientAnswers (int QuestionID) {
            List<AnswerModel> answers = new List<AnswerModel> ();
            answers = (from A in db.Answers join u in db.users on A.UserId equals u.UserId where u.Type == false && A.QuestionId == QuestionID orderby A.Date descending select new AnswerModel {
                Answer = A.Answer,
                    UserName = u.UserName,
                    Date = A.Date,
                    id = A.AnswerId,
                    UserType = u.Type,
                    userID = A.UserId,
                    role = u.Type,
                    //  role = db.users.Where(a => a.UserId == A.UserId).ToList().FirstOrDefault().Type,

            }).ToList ();

            return answers;
        }

        //List<QuestionModel> IDiabetes.GetAllQuestions(int n,int ID)
        //{

        //    List<QuestionModel> questions = new List<QuestionModel>();
        //    var Q = (from q in db.Questions
        //                 join u in db.users
        //                 on q.UserId equals u.UserId
        //                 where q.QuestionId<ID
        //                 orderby q.Date descending
        //                 select new QuestionModel
        //                 {
        //                     ID = q.QuestionId,
        //                     UserName = u.UserName,
        //                     Date = q.Date,
        //                     Question = q.Question,
        //                     answers = GetAnswers(q.QuestionId)

        //                 });
        //    if (n == -1) questions = Q.Take(n).ToList();
        //    else questions = Q.ToList();
        //    return questions;
        //}

        //    public List<QuestionModel> GetQuestionsDocAnswers(int n,int ID)
        //    {
        //            List<QuestionModel> questions = new List<QuestionModel>();
        //        IQueryable<QuestionModel> Q;
        //        if (ID != -1)
        //        {
        //             Q = (from q in db.Questions
        //                     join u in db.users
        //                     on q.UserId equals u.UserId
        //                     where q.QuestionId < ID
        //                     orderby q.Date descending
        //                     select new QuestionModel
        //                     {
        //                         ID = q.QuestionId,
        //                         UserName = u.UserName,
        //                         Date = q.Date,
        //                         Question = q.Question,
        //                         answers = GetDoctorAnswers(q.QuestionId)

        //                     });
        //        }
        //        else
        //        {
        //             Q = (from q in db.Questions
        //                     join u in db.users
        //                     on q.UserId equals u.UserId

        //                     orderby q.Date descending
        //                     select new QuestionModel
        //                     {
        //                         ID = q.QuestionId,
        //                         UserName = u.UserName,
        //                         Date = q.Date,
        //                         Question = q.Question,
        //                         answers = GetDoctorAnswers(q.QuestionId)

        //                     });
        //        }
        //        if (n != -1) questions = Q.Take(n).ToList();
        //        else questions = Q.ToList();
        //        return questions;
        //    }
        //}

        public Answers AddAnswer (int userid, int questionid, string answer) {
            return db.Answers.FromSqlRaw ("AddAnswer {0},{1},{2},{3}", questionid, answer, 3, userid).AsEnumerable ().FirstOrDefault ();
        }

        public int FollowDoctor (int patientid, int doctorid, short AccessMedicalInfo) {
            return db.Database.ExecuteSqlRawAsync ("EXEC dbo.FollowDoctor {0},{1},{2},{3}", patientid, doctorid, AccessMedicalInfo, 2).Result;
        }

        public Questions AddQuestion (int doctorid, int patientid, string question) {
            return db.Questions.FromSqlRaw ("addquestion {0},{1},{2},{3}", doctorid, question, patientid, 0).AsEnumerable ().FirstOrDefault ();
        }
        public PatientFullModel GetPatientProfile (int id) {
            var sp = new PatientFullModel ();

            var u = db.users.Find (id);
            sp.userID = u.UserId;
            sp.user_name = u.UserName;
            sp.img = u.ImageSource;
            var p = db.Patient.Find (id);
            sp.medical_cond = p.MedicalCondetion;
            sp.birth_date = p.BirthDate;
            sp.gender = p.Gender;
            sp.weight = p.Weight;
            sp.height = p.Height;
            sp.life_style = p.LifeStyle;
            sp.points = p.Points;
            var t = db.Test.Where (a => a.PatientId == id).ToList ().OrderByDescending (a => a.Date).ToList ().FirstOrDefault ();
            if (t != null) {
                sp.testType = t.Type;
                sp.testResult = t.Result;
                sp.testMedicationsState = t.Medication;
                sp.testDate = t.Date;
            }
            var c = db.ChecksUps.Where (a => a.PatientId == id && a.Status == 1).OrderByDescending (a => a.Date).ToList ().FirstOrDefault ();
            if (c != null) {

                sp.checkupData = c.ResultData;
                sp.checkupType = c.CheckupType;
                sp.checkupNotes = c.Notes;
                sp.checkupStatus = c.Status;
                sp.checkupDate = c.Date;

            }
            sp.drugs = (from d in db.Drugs join dp in db.DrugPatient on d.DrugId equals dp.DrugId where dp.PatientId == id select new DrugModel {
                drugId = d.DrugId,
                    drugName = d.DrugName,
                    dosage = dp.Dosage,
                    imageSource = d.ImageSource,
                    note = dp.Note,
                    patientId = id

            }).ToList ();
            return sp;
        }

        //List<QuestionModel> IDiabetes.GetAllQuestions(int n,int ID)
        //{

        //    List<QuestionModel> questions = new List<QuestionModel>();
        //    var Q = (from q in db.Questions
        //                 join u in db.users
        //                 on q.UserId equals u.UserId
        //                 where q.QuestionId<ID
        //                 orderby q.Date descending
        //                 select new QuestionModel
        //                 {
        //                     ID = q.QuestionId,
        //                     UserName = u.UserName,
        //                     Date = q.Date,
        //                     Question = q.Question,
        //                     answers = GetAnswers(q.QuestionId)

        //                 });
        //    if (n == -1) questions = Q.Take(n).ToList();
        //    else questions = Q.ToList();
        //    return questions;
        //}

        //    public List<QuestionModel> GetQuestionsDocAnswers(int n,int ID)
        //    {
        //            List<QuestionModel> questions = new List<QuestionModel>();
        //        IQueryable<QuestionModel> Q;
        //        if (ID != -1)
        //        {
        //             Q = (from q in db.Questions
        //                     join u in db.users
        //                     on q.UserId equals u.UserId
        //                     where q.QuestionId < ID
        //                     orderby q.Date descending
        //                     select new QuestionModel
        //                     {
        //                         ID = q.QuestionId,
        //                         UserName = u.UserName,
        //                         Date = q.Date,
        //                         Question = q.Question,
        //                         answers = GetDoctorAnswers(q.QuestionId)

        //                     });
        //        }
        //        else
        //        {
        //             Q = (from q in db.Questions
        //                     join u in db.users
        //                     on q.UserId equals u.UserId

        //                     orderby q.Date descending
        //                     select new QuestionModel
        //                     {
        //                         ID = q.QuestionId,
        //                         UserName = u.UserName,
        //                         Date = q.Date,
        //                         Question = q.Question,
        //                         answers = GetDoctorAnswers(q.QuestionId)

        //                     });
        //        }
        //        if (n != -1) questions = Q.Take(n).ToList();
        //        else questions = Q.ToList();
        //        return questions;
        //    }
        //}
        public PatientFullModel UpdatePatientProfile (int id, PatientFullModel sp) {
            var u = db.users.Where (u => u.UserId == id).ToList ().FirstOrDefault ();
            u.UserId = sp.userID;
            u.UserName = sp.user_name;
            u.ImageSource = sp.img;

            db.SaveChanges ();

            var p = db.Patient.Where (a => a.PatientId == id).ToList ().FirstOrDefault ();
            p.MedicalCondetion = (short) sp.medical_cond;
            sp.birth_date = p.BirthDate;
            p.Gender = sp.gender;
            p.Weight = (short) sp.weight;
            p.Height = (short) sp.height;
            p.LifeStyle = (short) sp.life_style;
            // p.Points = (short)sp.points;
            db.SaveChanges ();

            var t = db.Test.Where (a => a.PatientId == id).ToList ().LastOrDefault ();
            if (t != null) {
                t.Type = (short) sp.testType;
                t.Result = sp.testResult;
                t.Medication = sp.testMedicationsState;
                t.Date = sp.testDate;
            }
            var c = db.ChecksUps.Where (a => a.PatientId == id).ToList ().LastOrDefault ();
            if (c != null) {

                c.ResultData = sp.checkupData;
                c.CheckupType = (short) sp.checkupType;
                c.Notes = sp.checkupNotes;
                c.Status = sp.checkupStatus;
                c.Date = (DateTime) sp.checkupDate;

            }
            db.SaveChanges ();
            db.DrugPatient.RemoveRange (db.DrugPatient.Where (a => a.PatientId == id).ToList ());
            foreach (DrugModel drug in sp.drugs) {
                db.DrugPatient.Add (new DrugPatient () {
                    Dosage = drug.dosage,
                        DrugId = drug.drugId,
                        Note = drug.note,
                        PatientId = id
                });
            }
            db.SaveChanges ();
            return (GetPatientProfile (id));
        }

        public List<Users> getAllUsers () {

            return db.users.ToList ();
        }

        public ChecksUps DeleteCheckup (ChecksUps checkup) { //////////////////need change
            // db.Database.ExecuteSqlRaw("UpdateCheckups {0}", checkup);
            db.ChecksUps.Remove (checkup);
            db.SaveChanges ();
            return checkup;
        }

        public List<Drugs> GetAllDrugs (int PatientID) {
            return db.Drugs.ToList ();
        }
        public void savePost (UserSavedPosts p) {
            try {
                db.UserSavedPosts.Add (p);
                db.SaveChanges ();
            } catch { }

        }
        public patientSimpleModel getpatientprofile (int id) {
            var p = (from ptn in db.Patient join u in db.users on ptn.PatientId equals u.UserId where ptn.PatientId == id select new patientSimpleModel () {
                userName = u.UserName,
                    id = u.UserId,
                    imagesource = u.ImageSource,

            }).ToList ().FirstOrDefault ();
            return p;
        }

        public List<ChecksUps> GetPatientCheckups (int PatientID, int DoctorID) {
            short type = 1;
            List<ChecksUps> c = null;
            var x = db.PatientDoctorsFollow.Where (a => a.DoctorId == DoctorID && a.PatienId == PatientID).ToList ().FirstOrDefault ();
            if (x != null) {
                type = x.AccessMedicalInfo;
            }
            if (type == 2) {
                c = db.ChecksUps.Where (a => a.PatientId == PatientID).ToList ();
            }
            return c;

        }
        public List<Test> getAllTestswithouttype (int patientid) {
            return db.Test.FromSqlRaw ("SelectTests {0} ", patientid).ToList ();

        }

        public Msg AddMsg (string content, int ChatID, int UserID) {
            var X = db.Msg.Add (new Msg () { ChatId = ChatID, UserId = UserID, MsgContent = content, Date = DateTime.Now }).Entity;
            db.SaveChanges ();
            return X;
        }

    }
}
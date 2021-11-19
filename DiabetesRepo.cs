using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.ModelViews;
using static WebApplication.Repo.IDiabetes;

namespace WebApplication.Repo
{
        public partial class DiabetesRepo : IDiabetes
    {
        DiabetesSystem2Context db;

        public DiabetesRepo(DiabetesSystem2Context _db)
        {
            db = _db;
        }

        public Posts AddPost(Posts p)
        {
            return db.Posts.FromSqlRaw("AddPost {0}, {1},{2},{3},{4}", p.UserId, p.CategoryId, p.ImageSource, p.PostContent, p.ReactionId).AsEnumerable().FirstOrDefault();
        }

        public List<Test> GetAllTests(int patientid, short type)
        {
            return db.Test.FromSqlRaw("SelectTestsByType {0},{1} ", patientid,type).ToList();

        }

        public DrugPatient AddDrug(int drugid, int patientid, string note, int dose)
        {
          return  db.DrugPatient.FromSqlRaw("AddDrugForPatient {0},{1},{2},{3}", drugid, patientid, note, dose).AsEnumerable().FirstOrDefault();
        }

        public ChecksUps UpdateCheckup(ChecksUps checkup)
        {//////////////////need change
         // db.Database.ExecuteSqlRaw("UpdateCheckups {0}", checkup);
            db.ChecksUps.Update(checkup);
            db.SaveChanges();
            return checkup;
        }

        public List<Questions> GetMyQuestions(int userId)
        {
            return db.Questions.FromSqlRaw("Questions_Select {0}", userId).ToList();
        }

        public Questions AddQuestion(int doctorid, int patientid, string question)
        {
            return db.Questions.FromSqlRaw("addquestion {0}", doctorid, patientid, question).FirstOrDefault();
        }

        public Answers AddAnswer(int userid, int questionid, string answer)
        {
            return db.Answers.FromSqlRaw("AddAnswer {0},{1},{2}", questionid, answer,userid).AsEnumerable().FirstOrDefault();
        }

        public List<ChecksUps> GetPatientCheckups(int patientid)
        {
            return db.ChecksUps.FromSqlRaw("ChecksUps_Select {0}", patientid).ToList();
        }

        public List<Drugs> GetAllDrugs()
        {
            return db.Drugs.ToList();
        }


        public Certificates AddCertificate(int doctorid, string certificate, string university)
        {
          return  db.Certificates.FromSqlRaw("AddCertificate  {0}, {1} ,{2}  ", certificate, university, doctorid).ToList().FirstOrDefault();
        }

        public ChecksUps AddCheckup(ChecksUps checksUp)
        {
           var x = db.ChecksUps.Add(checksUp);
            db.SaveChanges();
            return x.Entity;
            // db.ChecksUps.FromSqlRaw("addcheckup  {0}, {1} ,{2}", patientid, checkuptype, notes).ToList();
        }


        public Test AddTest(int result, short type, int patientid, bool medication) //edited to void, may return testID
        {
           return  db.Test.FromSqlRaw("EXEC dbo.AddTest {0},{1},{2},{3}", result, type, patientid, medication).AsEnumerable().FirstOrDefault();
            //int test_id = db.Test.OrderByDescending(p => p.Id).FirstOrDefault().Id;
        }

        public int FollowDoctor(int patientid, int doctorid, short AccessMedicalInfo)
        {
           return   db.Database.ExecuteSqlRawAsync("EXEC dbo.FollowDoctor {0},{1},{2},{3}", patientid, doctorid, AccessMedicalInfo, 2).Result;

        }
        public List<Category> GetAllCategories()
        {
            return db.Category.ToList<Category>();
        }



        public List<Reactions> GetAllReacts()
        {
            return db.Reactions.ToList();
        }


        public List<Notification> GetDoctorNOtifications(int doctorid)
        {
            return db.Notification.FromSqlRaw("Notification_Doctor_Select  {0}", doctorid).ToList();
        }

        public List<GetMyDoctors> GetMyDoctors(int? patient_id)
        {
            var sp = from f in db.PatientDoctorsFollow
                     join u in db.users
                     on f.DoctorId equals u.UserId
                     where f.PatienId == patient_id
                     select new GetMyDoctors
                     {
                         id = f.Id,
                         doctor_id = f.DoctorId,
                         access_med_info = f.AccessMedicalInfo,
                         user_id = u.UserId,
                         user_name = u.UserName,
                         img = u.ImageSource,
                         type = u.Type,
                         identity_id = u.ID
                     };
            return sp.ToList();
        }

        public List<FollowingPatients> GetMyPatients(int doctorid)
        {
            var sp = from u in db.users
                     join p in db.Patient
                     on u.UserId equals p.PatientId
                     join f in db.PatientDoctorsFollow
                     on p.PatientId equals f.PatienId
                     where f.DoctorId == doctorid
                     select new FollowingPatients
                     {
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
            return sp.ToList();
        }


        public List<Notification> GetPatientNotifications(int patientid)
        {
            return db.Notification.FromSqlRaw("EXECUTE dbo.Notification_Patient {0}", patientid).ToList();
        }


        public List<Posts> GetPosts(int CategoryId)
        {
            return db.Posts.FromSqlRaw("Posts_Select  {0}", CategoryId).ToList();
        }

        public List<SavedPosts> GetSavedPosts(int userID)
        {
            // return db.savedposts.FromSqlRaw($"SavedPosts {userID}").ToList();
            var sp = from p in db.Posts
                     join u in db.UserSavedPosts
                     on p.PostId equals u.PostId
                     where p.UserId == userID
                     select new SavedPosts
                     {
                         post_id = p.PostId,
                         user_id = p.UserId,
                         category_id = p.CategoryId,
                         img = p.ImageSource,
                         content = p.PostContent,
                         react_id = p.ReactionId,
                         date = p.PostDate
                     };
            return sp.ToList();
        }

        public List<Questions> GetSavedQuestions(int userId)
        {
            return db.Questions.FromSqlRaw("SavedQuestions_Select  {0}", userId).ToList();
        }

        public bool UnfollowDoctor(int patientid, int doctorid)
        {
            bool flag = false;
            int x = db.Database.ExecuteSqlRaw("unfollowdoctor {0}, {1}", doctorid, patientid);
            if (x != 0) flag = true;
            return flag;
        }
  
        public int GetUserID(string ID)
        {
            return db.users.Where(a => a.ID == ID).SingleOrDefault().UserId;
        }


        public GetMyDoctors Update_access_medicalInfo(int followID, short status)
        {
            var data = (from Id in db.PatientDoctorsFollow
                        where followID == Id.Id
                        select Id).First();
            data.AccessMedicalInfo = status;
            GetMyDoctors x = new GetMyDoctors { access_med_info = data.AccessMedicalInfo, id = data.Id, doctor_id = data.DoctorId };
            db.SaveChanges();
            return x;
        }


        public List<Posts> GetAllPosts()
        {
            return db.Posts.FromSqlRaw("getLatestPosts ").ToList();
        }



        public List<Posts> GetPostsByUserID(int user_id)
        {
            return db.Posts.FromSqlRaw("Posts_Select_ByUserID  {0}", user_id).ToList();
        }
        public List<Posts> GetPosts(int postID, List<int> CategoryId)
        {
           
            List<Posts> post_list2 = new List<Posts>();
            if (postID == -1)
            {

                    var posts = (from p in db.Posts
                                 orderby p.PostDate descending
                                 where CategoryId.Contains(p.CategoryId)
                                 select p).Take(10).ToList();
                    post_list2.AddRange(posts);
                   

            }
            else
            {


               
                    var posts = (from p in db.Posts
                                 orderby p.PostDate descending
                                 where p.PostId > postID && CategoryId.Contains(p.CategoryId)
                                 select p).Take(10).ToList();
                
                post_list2.AddRange(posts);
                }
            return post_list2;
        }


        public Patient GetPatient(int id)
        {
            return db.Patient.FirstOrDefault(a => a.PatientId == id);
        }

        public Doctor GetDoctor(int id)
        {
            return db.Doctor.FirstOrDefault(a => a.DoctorId == id);
        }

        public Users GetUser(int id)
        {

            return db.users.FirstOrDefault(a => a.UserId == id);
        }


        public List<Questions> GetQuestionsByUser(int id)
        {
            return db.Questions.ToList().FindAll(a => a.UserId == id);
        }




        //////////////////////////////////////////////////////////////
        /// 
        public List<Questions> getMentionedInQuestions(int drid)
        {
            List<QuestionModel> doctorquestions = new List<QuestionModel>();
            var query = from q in db.Questions
                        join a in db.QuestionDoctorsMention
                        on q.QuestionId equals a.QuestionId
                        where a.DoctorId == drid
                        select q;
            return query.ToList();
        }

       public List<Questions> getAnsweredQuestions(int drid)
        {
            var query = from q in db.Questions
                        join a in db.Answers
                        on q.QuestionId equals a.QuestionId
                        where a.UserId == drid
                        select q;
            return query.ToList();

        }

       public List<Certificates> GetCertificates(int DrID)
        {
            return db.Certificates.ToList().FindAll(c => c.DoctorId == DrID);
        }


        public int request_access_medicalInfo(int drid, int pid,int FollowID)
        {
            ///0 refused ,1 requested ,2 approved ,3 null 
            
            return db.Database.ExecuteSqlRaw("RequestMedicalInfo {0},{1},{2}", drid, pid, FollowID);
        }


       public List<AnswerModel> GetAnswers(int QuesId)
       {
         List<Answers> answers =  db.Answers.ToList().FindAll(c => c.QuestionId == QuesId);
            List<AnswerModel> answers1 = new List<AnswerModel>();
            for (int i =0;i<answers.Count; i++)
            {
                AnswerModel answer = new AnswerModel();
                answer.Answer = answers[i].Answer;
                answer.UserName = GetUser(answers[i].UserId).UserName;
                answer.Date = answers[i].Date;

                answers1.Add(answer);

            }
            return answers1;
       }
    }
}
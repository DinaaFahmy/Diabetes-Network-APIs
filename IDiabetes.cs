using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.ModelViews;

namespace WebApplication.Repo
{
    public interface IDiabetes
    {


            List<Posts> GetPostsByUserID(int user_id);
            List<Posts> GetAllPosts();
            GetMyDoctors Update_access_medicalInfo(int followID, short status);

        List<Posts> GetPosts(int postID, List<int> CategoryId);


            Posts AddPost(Posts posts);
            List<Category> GetAllCategories();
            List<Reactions> GetAllReacts();

            List<Test> GetAllTests(int patientid, short type);
            List<Notification> GetPatientNotifications(int patientid);
            List<Notification> GetDoctorNOtifications(int doctorid);

            List<ChecksUps> GetPatientCheckups(int patientid);

            List<Drugs> GetAllDrugs();
            DrugPatient AddDrug(int drugid, int patientid, string note, int dose);

            List<GetMyDoctors> GetMyDoctors(int? patientid);

            ChecksUps AddCheckup(ChecksUps checksUp);


            ChecksUps UpdateCheckup(ChecksUps checkup);

            List<SavedPosts> GetSavedPosts(int userId);
            List<Questions> GetSavedQuestions(int userId);

            List<Questions> GetMyQuestions(int userId);

             int FollowDoctor(int patientid, int doctorid, short AccessMedicalInfo);

            bool UnfollowDoctor(int patientid, int doctorid);

            Questions AddQuestion(int doctorid, int patientid, string question);

            //doctor
            List<FollowingPatients> GetMyPatients(int doctorid);

            Certificates AddCertificate(int doctorid, string certificate, string university);
            Answers AddAnswer(int userid, int questionid, string answer);
            //Mine
            Test AddTest(int result, short type, int patientid, bool medication);//change result in db to int **DONE**

            int GetUserID(string ID);
            /////////////////////////////////////////////////////////////////////////
            ///
            Patient GetPatient(int ID);

            Patient UpdatePatient(Patient patient);

            List<Test> GetAllTests(int PatientID);
        //////////////////////////////////////////////////////////////////////////////
        ///Patient GetPatient(int id);
        Doctor GetDoctor(int id);
        Users GetUser(int id);
        List<Questions> GetQuestionsByUser(int id);
        //////////////////////////////////////////////////////////////
        ///
        List<Questions> getMentionedInQuestions(int drid);

        List<Questions> getAnsweredQuestions(int drid);

        List<Certificates> GetCertificates(int DrID);



        int request_access_medicalInfo(int drid, int pid, int FollowID);
        ///////////////////////////////////////////////////////////////////////
        ///

        List<AnswerModel> GetAnswers(int QuesId);
    }
}

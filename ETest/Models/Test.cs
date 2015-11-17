using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    public enum TestType
    {
        [Description("Bài kiểm tra nộp tập tin")]
        Upload,
        [Description("Bài kiểm tra trắc nghiệm")]
        Test
    }
    public enum GradeType
    {
        [Description("Điểm lớn nhất")]
        Maximum,
        [Description("Điểm trung bình")]
        Medium,
        [Description("Điểm nhỏ nhất")]
        Minimum
    }


    public class Test
    {
        

        public long TestId { get; set; }
        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string TestTitle { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Mô tả")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime TestStart { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Thời gian kết thúc")]
        public DateTime TestEnd { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Thời gian làm bài")]
        public int TestTime { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Khóa học")]
        public long CourseId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Số lần nộp bài")]
        public int SubmitNo { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Loại bài kiểm tra")]
        public TestType TestType { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tổng điểm")]
        public int Score { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Trộn câu hỏi")]
        public bool MixedQuestions { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Cách tính điểm")]
        public GradeType GradeType  { get; set; }
        [NotMapped]
        public int SubmitNoUser { get; set; }
        

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Course Course { get; set; }
        public virtual IList<TestDetail> TestDetails { set; get; }
        public virtual IList<AnswerSheet> AnswerSheets { get; set; }

        public Test()
        {

        }

        public Test(string data)
        {
            var jObject = JObject.Parse(data);
            TestId = DataUtil.ToLong(jObject["id"]);
            TestTitle = (string)jObject["title"];
            Description =  (string)jObject["description"];
            CourseId = DataUtil.ToLong(jObject["course"]);
            TestStart = DataUtil.ToDateTime((string)jObject["timeStart"], "dd/MM/yyyy HH:mm");
            TestEnd = DataUtil.ToDateTime((string)jObject["timeEnd"], "dd/MM/yyyy HH:mm");
            TestTime = DataUtil.ToInt(jObject["testTime"]);
            SubmitNo = DataUtil.ToInt(jObject["submitNo"]);
            Actived = DataUtil.ToBool(jObject["actived"]);
            Score = 100;
            MixedQuestions = DataUtil.ToBool(jObject["mixedQuestion"]);
            GradeType = (GradeType)DataUtil.ToInt(jObject["gradeType"]);
            var testDetailJsons = jObject["details"].ToArray();

            var testDetails = testDetailJsons.Select(test => new TestDetail(test)).ToList();

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            TestDetails = testDetails;
        }


        public void SetAnswer(AnswerSheet answerSheet)
        {
            foreach (var answer in answerSheet.Answers)
            {
                var question = TestDetails.First(s => s.QuestionId == answer.QuestionId).Question;
                foreach (var detail in question.QuestionDetails)
                {
                    var answerDetail = answer.AnswerDetails.Find(s => s.QuestionDetailId == detail.QuestionDetailId);
                }
            }
        }
    }
}
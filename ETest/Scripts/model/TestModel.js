function TestModel(id, title, description, course, timeStart, timeEnd, testTime, mixedQuestion, submitNo, gradeType, actived, details) {
    this.id = id;
    this.title = title;
    this.description = description;
    this.course = course;
    this.timeStart = timeStart;
    this.timeEnd = timeEnd;
    this.testTime = testTime;
    this.mixedQuestion = mixedQuestion;
    this.submitNo = submitNo;
    this.gradeType = gradeType;
    this.actived = actived;
    this.details = details;
}

function TestDetailModel(id, score, order, details) {
    this.id = id;
    this.score = score;
    this.order = order;
    this.details = details;
}

function QuestionDetail(id, score) {
    this.id = id;
    this.score = score;
}
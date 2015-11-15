function AnswerSheet(id, testDetails) {
    this.id = id;
    this.testDetails = testDetails;
}

function TestDetail(id,questionDetail) {
    this.id = id;
    this.questionDetail = questionDetail;
}
function QuestionDetail(id, type, answer) {
    this.id = id;
    this.type = type;
    this.answer = answer;
}

function AssociateAnswer(rightId,leftId) {
    this.rightId = rightId;
    this.leftId = leftId;
}

function GapAnswer(index, answer) {
    this.index = index;
    this.answer = answer;
}
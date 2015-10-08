function OrderQuestion(id, orderNo, content, result) {
    this.id = id;
    this.orderNo = orderNo;
    this.content = content;
    this.result = result;
}

function sortByResult(a, b) {
    var aName = parseInt(a.result);
    var bName = parseInt(b.result);
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
}
function dynamicSort(property) {
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }
    return function (a, b) {
        var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
        return result * sortOrder;
    }
}
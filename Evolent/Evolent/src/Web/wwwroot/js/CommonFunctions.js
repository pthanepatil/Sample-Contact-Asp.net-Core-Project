var baseURL = $("#BaseUrl").data("baseurl");

function GetBaseUrl() {
    //return "../" + $("#BaseUrl").data("baseurl");
    return '';
}

function GenericGridPager(PageContainer, PageURL, PageSize, WaitContainer) {
    this.PageContainer = PageContainer;
    this.PageURL = PageURL;
    this.PageSize = PageSize;
    this.WaitContainer = WaitContainer;
}

GenericGridPager.prototype.FetchData = function (ForPage) {
    $("#" + this.PageContainer).show();
    var objThis = this;
    var objectData = this.ObjectData;
    var finalURL = this.PageURL;
    if (finalURL.indexOf("?") > 0)
        finalURL = finalURL + "&";
    else
        finalURL = finalURL + "?";
    
    finalURL = GetBaseUrl() + finalURL + "PageNo=" + ForPage + "&PageSize=" + this.PageSize + "&SortBy=" + this.SortBy;

    $.ajax({
        url: finalURL,
        type: "Get",
        dataType: 'html',
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (data) {
            $("#" + objThis.PageContainer).html("");
            $("#" + objThis.PageContainer).html(data);
        },
        error: function (data) {
        },

    });
}

GenericGridPager.prototype.SetPagerClickEvent = function () {
    var objThis = this;
    var pageHolderBtn = "#" + this.PageContainer + " #grdpager .pagination-button";
    $(document).on("click", pageHolderBtn, function () {

        var newPageNo = $(this).attr("data-page");
        objThis.FetchData(newPageNo);
    });
}

//*------------------Dashboard-------------*//
function DashboardView() {
    this.ContactGrid = new GenericGridPager("divUserContacts", '../Dashboard/GetContacts', 15, "waiting_result");
    this.ContactGrid.FetchData(1);
    this.ContactGrid.SetPagerClickEvent();
}
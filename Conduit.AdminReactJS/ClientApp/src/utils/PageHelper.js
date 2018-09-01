class Page{
    constructor(nextPageLink,pageSize,previousPageLink,totalCount,currentPage,totalPages){
        this.nextPageLink = nextPageLink;
        this.pageSize = pageSize;
        this.previousPageLink = previousPageLink;
        this.totalCount = totalCount;
        this.currentPage=currentPage;
        this.totalPages = totalPages;
    }
}
class PageHelper{
    constructor(xpage){
        this.xpage=xpage;
    }
    getPage(){
        var pagination = JSON.parse(this.xpage);
        if (pagination != null) {
            
            return new Page(pagination.nextPageLink, pagination.pageSize,pagination.previousPageLink,pagination.totalCount,pagination.currentPage,pagination.totalPages);
        }
        
        return new Page("",1,"",1,1,1);
    }
}
export {PageHelper,Page}
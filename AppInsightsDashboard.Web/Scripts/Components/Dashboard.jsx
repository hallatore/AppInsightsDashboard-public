var Dashboard = React.createClass({
    render: function () {
        var projects = this.props.projects.map(function (item, i) {
            if (item.itemType == 0)
                return (<DashboardSiteItem key={i} name={item.name} url={item.url} silent={item.silent} />);

            if (item.itemType == 1)
                return (<DashboardAnalyticsItem key={i} name={item.name} url={item.url} silent={item.silent} queries={item.queries} />);

            return null;
        }.bind(this));
        
        return(
            <div className="el-dashboard clearfix">
                <ul className={"projects columns-" + this.props.columns}>
                    {projects}
                </ul>
            </div>
        );
    }
});
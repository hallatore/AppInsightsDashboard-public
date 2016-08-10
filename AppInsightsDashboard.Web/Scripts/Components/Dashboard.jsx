var Dashboard = React.createClass({
    render: function () {
        var projects = this.props.projects.map(function(item, i) {
            return (<DashboardItem key={i} name={item.name} url={item.url} />);
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
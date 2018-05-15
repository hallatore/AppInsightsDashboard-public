var DashboardSiteItem = React.createClass({
    getInitialState: function () {
        return {
            data: {},
            isError: true,
            errorMessage: "Loading data ..."
        };
    },
    componentDidMount: function () {
        this.load();
        setTimeout(function() {
            setInterval(this.load, 60 * 1000);
        }.bind(this), Math.random() * 60 * 1000);
    },
    render: function () {
        var error = this.state.isError ? (<div className="error">{this.state.errorMessage}</div>) : null;
        return (
            <li className="el-dashboardItem">
                <div className={"el-box clearfix " + this.getErrorLevel(this.state.data.ErrorLevel)}>
                    <h2>{this.props.name}</h2>
                    <div className="el-status">
                        <div className="title">Requests</div>
                        <div className="value">{this.state.data.RequestsPerMinute}<span>rpm</span></div>
                    </div>
                    <div className={"el-status " + this.getErrorLevel(this.state.data.AvgResponseTimeErrorLevel)}>
                        <div className="title">90 Percentile</div>
                        <div className="value">{this.state.data.AvgResponseTime}<span>ms</span></div>
                    </div>
                    <div className={"el-status " + this.getErrorLevel(this.state.data.ErrorRateLevel)}>
                        <div className="title">Error rate</div>
                        <div className="value">{this.state.data.ErrorRate}<span>%</span></div>
                    </div>
                    <div className={"el-status " + this.getErrorLevel(this.state.data.ErrorRateLevel10Min)}>
                        <div className="title">Error rate (10 min)</div>
                        <div className="value">{this.state.data.ErrorRate10Min}<span>%</span></div>
                    </div>
                    {error}
                </div>
            </li>
        );
    },
    getErrorLevel: function (errorLevel) {
        if (errorLevel === 3)
            return "gray";

        if (this.props.silent === true)
            return "";

        if (errorLevel === 2)
            return "red";

        if (errorLevel === 1)
            return "yellow";

        return "";
    },
    load: function () {
        $.ajax({
            url: this.props.url,
            success: function (data) {
                this.setState({ data: data, isError: false });
            }.bind(this),
            error: function() {
                this.setState({ isError: true, errorMessage: "Error getting results" });
            }.bind(this)
        });
    }
});
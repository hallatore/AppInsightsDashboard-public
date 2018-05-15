var DashboardAnalyticsItem = React.createClass({
    getInitialState: function () {
        return {
            data: { Values: []},
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
        var values = this.state.data.Values.map(function (item, index) {
            return (
                <div key={index} className={"el-status " + this.getErrorLevel(item.ErrorLevel)}>
                    <div className="title">{item.Name}</div>
                    <div className="value">{item.Value}<span>{item.Postfix}</span></div>
                </div>
                );
        }.bind(this));

        return (
            <li className={"el-dashboardItem queries_" + this.props.queries}>
                <div className={"el-box clearfix " + this.getErrorLevel(this.state.data.ErrorLevel)}>
                    <h2>{this.props.name}</h2>
                    {values}
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
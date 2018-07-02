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
    componentDidUpdate: function() {
        for (var i = 0; i < this.state.data.Values.length; i++) {
            var item = this.state.data.Values[i];

            if (item.Type === "Chart") {
                this.drawChart(this.refs["chart_" + i], item.Value, item.ErrorLevel);
            }
        }
    },
    drawChart: function(canvas, values, errorLevel) {
        if (canvas.getContext) {
            var ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.fillStyle = this.getErrorLevel(errorLevel) || "#555";
            var width = canvas.width / values.length;
            var height = canvas.height;

            for (var i = 0; i < values.length; i++) {
                var itemHeight = height * values[i];
                ctx.fillRect(i * width, height - itemHeight, width, itemHeight);
            }
        }
    },
    render: function () {
        var error = this.state.isError ? (<div className="error">{this.state.errorMessage}</div>) : null;
        var values = this.state.data.Values.map(function (item, index) {
            if (item.Type === "Value") {
                return (
                    <div key={index} className={"el-status " + this.getErrorLevel(item.ErrorLevel)}>
                        <div className="title">{item.Name}</div>
                        <div className="value">{item.Value}<span>{item.Postfix}</span></div>
                    </div>
                );
            }
            else if (item.Type === "Chart") {
                return (
                    <div key={index} className={"el-status " + this.getErrorLevel(item.ErrorLevel)}>
                        <canvas className="chart" ref={"chart_" + index}></canvas>
                    </div>
                );
            }

            return null;
        }.bind(this));

        return (
            <li className={"el-dashboardItem queries_" + this.props.queries}>
                <div className={"el-box clearfix " + this.getErrorLevel(this.state.data.ErrorLevel)}>
                    <h2>{this.props.name}</h2>
                    <div className="status-container">
                        {values}
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
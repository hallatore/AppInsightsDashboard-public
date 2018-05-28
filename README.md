# Dashboard 

A dashboard to monitor your websites and services that are using **Azure Application Insights**.

The dashboard supports monitoring based on live telemetry and webtests that run custom queries.

Url: `http://<base-url>/<dashboard guid>/2`

Example: `http://localhost:64416/b567dae6c-9da7-4c17-8e49-78e70010fe30/2`

## Configuration

### Adding a new Azure Application Insights resource

#### Generate an API key

You need an API key in order to query the Application Insights data:

1. Log in to the [Azure Portal](https://portal.azure.com).
2. Navigate to the **Application Insights** Azure resource that you want to add.
3. Copy the **Application ID** and save it for the next section.
3. Click on setting **Configure -> API Access** on the left-hand navigation pane, and click on **Create API key** at the top of the opened blade.
4. Provide the following details and click on **Generate key** when done:
* Name: **`<your website/service name>` Dashboard**
* Permission: **Read telemetry**
5. Copy the **Key** and save it for the next section.

#### Add an Application Insights item to the dashboard

In order to monitor your website and service, you need to add configuration to the dashboard in order to fetch the website's/service's Application Insights data:

1. Open `Config.cs`.
2. Add a new entry to the `mainDashboard` dictionary:
* Key: `<your website/service name>`
* Value: a new `DashboardSite` object.
3. Create a new `ApiToken` object: 
* Paste in the **Application ID** from previous section as `applicationId`. 
* Paste in the **Key** from the previous section as `apiKey`.
4. Add the `apiToken` object to your new `DashboardSite` object.

Run the project and navigate to your dashboard to view the Application Insights data.
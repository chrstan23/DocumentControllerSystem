function DashboardCard({ label, value }) {
    return(
        <div
            style={{
                display: "inline-block",
                border: "1px solid #ccc",
                padding: "12px 20px",
                margin: "6px",
                minWidth: "140px",
                textAlign: "center",
            }}
        >
            <h3 style={{margin:0}}>{value}</h3>
            <p style={{margin:0}}>{label}</p>
        </div>
    );
}

export default DashboardCard;
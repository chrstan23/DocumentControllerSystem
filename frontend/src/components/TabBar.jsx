import { NavLink } from "react-router";

const TABS = [
    { to: "/", label: "Dashboard", end: true },
    { to: "/masterlist", label: "Document Masterlist" },
    { to: "/create-document", label: "Create Document" },
    { to: "/revision", label: "Revision" },
    { to: "/add-existing", label: "Add Existing Document" },
    { to: "/manage-users", label: "Manage Users" },

];

function TabBar() {
    return (
        <nav>
            {TABS.map((tab) => (
                <NavLink
                    key={tab.to}
                    to={tab.to}
                    end={tab.end}
                    style={({ isActive }) => ({
                        fontWeight: isActive ? "bold" : "normal",
                        marginRight: "16px",
                    })}
                >
                    {tab.label}
                    
                </NavLink>
            ))}
        </nav>
    );
}

export default TabBar;
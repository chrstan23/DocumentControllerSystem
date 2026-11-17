const TABS = [
    {id: "dashboard", label: "Dashboard" },
];

function TabBar({ activeTab, onChange }) {
    return (
        <nav>
            {TABS.map((tab) => (
                <button 
                key={tab.id}
                type="button"
                onClick={() => onChange(tab.id)}
                aria-current={tab.id === activeTab ? "page" : undefined}
                style={{fontWeight: tab.id === activeTab ? "bold" : "normal"}}
                >
                    {tab.label}
                </button>
            ))}
        </nav>
    );
}

export default TabBar;
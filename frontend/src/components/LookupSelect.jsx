function LookupSelect({ name, items, value, onChange, placeholder, disabled = false}) {
    return(
        <select name={name} value={value} onChange={onChange} required disabled={disabled}>
            <option value="">{placeholder}</option>
            {items.map((item) => (
                <option key={item.id} value={item.name}>
                    {item.name}
                </option>
            ))}
        </select>
    );
};

export default LookupSelect;
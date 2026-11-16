function LookupSelect({ name, items, value, onChange, placeholder }) {
    return(
        <select name={name} value={value} onChange={onChange} required>
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
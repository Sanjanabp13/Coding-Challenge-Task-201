const resolveReferences = (data) => {
    const idMap = {};
    data.forEach((item) => {
        if (item.$id) {
            idMap[item.$id] = item;
        }
    });
    return data.map((item) => {
        if (item.$ref) {
            return idMap[item.$ref];
        }
        return item;
    });
};

export default resolveReferences;

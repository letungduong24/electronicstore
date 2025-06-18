export const getPropertyLabel = (propertyName) => {
  const labels = {
    'ScreenSize': 'Kích thước màn hình',
    'Scope': 'Công suất',
    'Capacity': 'Dung tích'
  };
  return labels[propertyName] || propertyName;
};

export const getPropertyUnit = (propertyName) => {
  const units = {
    'ScreenSize': 'inch',
    'Scope': 'BTU',
    'Capacity': 'kg'
  };
  return units[propertyName] || '';
};

export const formatProductProperties = (properties) => {
  if (!properties || Object.keys(properties).length === 0) {
    return null;
  }

  return Object.entries(properties).map(([key, value]) => ({
    key,
    label: getPropertyLabel(key),
    value,
    unit: getPropertyUnit(key)
  }));
}; 
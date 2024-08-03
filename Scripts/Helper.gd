class_name Helper

#load files from path
static func loadAllResources(path: String): 
	var _resourceList = []
	var dir = DirAccess.open(path)
	
	if dir:
		dir.list_dir_begin()
		
		var _fileName = dir.get_next()
		while _fileName != "":
			if dir.current_is_dir():
				_fileName = dir.get_next()
				continue
			
			var _filePath = path + _fileName
			_filePath = _filePath.replace(".remap", "")
			var _resource = ResourceLoader.load(_filePath)
			
			if _resource:
				_resourceList.append(_resource)
			
			_fileName = dir.get_next()
		dir.list_dir_end()
	return _resourceList

#update dictionary
static func updateDictionary(value :Dictionary, new_value :Dictionary):
	var updatedDictionary = {}
	
	for key in new_value.keys():
		if value.has(key):
			updatedDictionary[key] = value[key]
		else:
			updatedDictionary[key] = new_value[key]
	
	return updatedDictionary

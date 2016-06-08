#coding=utf-8
import os
import sys
import codecs 
from string import Template
def script_path():
	import inspect, os
	caller_file = inspect.stack()[1][1]         # caller's filename
	return os.path.abspath(os.path.dirname(caller_file))# path
	
import xlrd
local = script_path()
xlsPath = os.path.join(local,"DBLogic.xls")
book = xlrd.open_workbook(xlsPath)

def ReadTemplate(file):
	path = os.path.join(local.decode('gbk').encode('utf8')+"/tmp/", file)
	f = open(path)
	text = ""
	for i in f:
		text += i
	return text

def createAndOpenFile(name):
	path = os.path.join(local.decode('gbk').encode('utf8')+"/src/", name)
	if os.path.exists(path):
		os.remove(path)
	return codecs.open(path, 'w+', 'gbk')	
	
def export(file, text, script):
	csv=createAndOpenFile(file)
	csv.write(text)
	csv.close()
	print script
	
class Interface:
	def __init__(self):
		self.functions = []
		self.name = ""
	def Declar(self):
		declar = ""
		for i in self.functions:
			declar += i.Declar()
		text = ReadTemplate("interface.cs")
		return text.replace("#I_Name#", self.name).replace("#Func_List#", declar)
	def ImplFuncs(self):
		funcs = ""
		for i in self.functions:
			funcs += i.Impl() 
		return funcs
	def StaticFun(self):		
		text = ReadTemplate("static_fun.cs")
		return text.replace("#I_Name#", self.name)
class FuncInfo:
	def __init__(self):
		self.script = ""
		self.name = ""
		self.returns = ""
		self.SQL_ID = ""
		self.paramers = ""
		self.paramers_code = ""
		self.operator = ""		
	def Declar(self):
		text = ReadTemplate("interface_fun.cs")
		return text.replace("#script#", self.script).replace("#name#", self.name).replace("#returns#", self.returns).replace("#paramers#", self.paramers)	
	def Impl(self):
		text = ReadTemplate("class_fun.cs")
		parm = self.paramers_code
		if len(parm) > 0:
			parm = " ," + parm            
		return text.replace("#script#", self.script).replace("#name#", self.name).replace("#returns#", self.returns).\
replace("#paramers#", self.paramers).replace("#operator#", self.operator).replace("#SQL_ID#", self.SQL_ID).\
replace("#paramers_code#", parm)

def read_val(val):
	if isinstance(val, float):
		return str(int(val))
	if isinstance(val, int):
		return str(val)
	return val.rstrip()
	
interfaces = {}
for sheet in book.sheets():
	interface = Interface()
	interface.name = sheet.name
	for i in range(1, sheet.nrows):
		func = FuncInfo()
		func.script = read_val(sheet.row(i)[0].value) 
		func.name = read_val(sheet.row(i)[1].value) 
		func.returns = read_val(sheet.row(i)[2].value) 
		func.SQL_ID = read_val(sheet.row(i)[3].value) 
		func.paramers = read_val(sheet.row(i)[4].value) 
		func.paramers_code = read_val(sheet.row(i)[5].value) 
		func.operator = read_val(sheet.row(i)[6].value) 
		interface.functions.append(func)
	interfaces[interface.name] = interface
    
def ImplFuncs():
	text = ReadTemplate("class.cs")
	funcs = ""
	infs = ""
	for i in interfaces.keys():
		funcs += interfaces[i].ImplFuncs()   
		if infs != "":
			infs += ", " 
		infs += i
	return text.replace("#Func_List#", funcs).replace("#interfaces#", infs)

def StaticClass():
	text = ReadTemplate("static.cs")
	funcs = ""
	for i in interfaces.keys():
		funcs += interfaces[i].StaticFun()   
	return text.replace("#Func_List#", funcs)
	
print ImplFuncs()
for i in interfaces.keys():
	#print interfaces[i].Declar()
	export(interfaces[i].name + ".cs", interfaces[i].Declar(), interfaces[i].name + " success")
export("Implements.cs", ImplFuncs(), "Implements success")
export("StaticHelper.cs", StaticClass(), "StaticHelper success")
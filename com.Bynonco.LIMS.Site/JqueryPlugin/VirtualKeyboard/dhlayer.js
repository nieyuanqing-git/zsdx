//定义popupMenu类
function dhlayer(){
	//author:dh20156
	this.content = null;
	this.background = "buttonface";
	this.border = "none";
	this.fontSize = "12px";
	this.padding = "2px;"
	this.cursor = "default";
	//popupMenu childNodes
	this.childNodes = new Array();
	//popupMenu body
	this.window = null;
	this.body = null;
	this.isOpen = false;

	//应用IFRAME以遮蔽顶层对象
	var ifr = document.createElement("IFRAME");
	ifr.style.zIndex = "-1";
	ifr.style.position = "absolute";
	ifr.style.top = "0px";
	ifr.style.left = "0px";
	ifr.style.border = "none";

	//应用DIV以遮蔽IFRAME的右键
	var il = document.createElement("DIV");
	il.style.overflowX = "hidden";
	il.style.overflowY = "hidden";
	il.style.zIndex = "999999999";
	il.style.position = "absolute";
	il.style.top = "0px";
	il.style.left = "0px";

	//定义popupMenu方式
	var layer = document.createElement("DIV");
	layer.style.overflow = "hidden";
	layer.style.padding = "0px";
	layer.style.border = "none";
	layer.style.zIndex = "99999999";
	layer.style.position = "absolute";
	layer.style.display = "none";
	layer.style.MozUserSelect = "none";
	layer.onselectstart = function(){return false};

	document.body.appendChild(layer);

	layer.appendChild(ifr);
	layer.appendChild(il);
	this.layer = layer;
	this.window = layer;
	this.childNodes = il.childNodes;
	this.body = il;


	//popupMenu的显示方法，参数w-width(popupMenu宽),h-height(popupMenu高),o-parent Object(popupMenu的父对象)
	this.init = function(l,t,w,h){

		l = l+"px";
		t = t+"px";
		w = w+"px";
		h = h+"px";

		il.style.padding = this.padding;
		il.style.fontSize = this.fontSize;
		il.style.cursor = this.cursor;
		il.style.background = this.background;
		il.style.width = w;
		il.style.height = h;
		ifr.style.width = w;
		ifr.style.height = h;
		layer.style.width = w;
		layer.style.height = h;
		layer.style.left = l;
		layer.style.top = t;
		layer.style.border = this.border;
		layer.style.background = this.background;

		layer.style.display = "block";
		il.innerHTML = this.content;
		this.isOpen = true;
	}

	this.show = function(l,t){
		layer.style.left = l;
		layer.style.top = t;
		layer.style.display = "block";
		this.isOpen = true;
	}

	this.hide = function(){
		layer.style.display = "none";
		this.isOpen = false;
	}

	this.esc = function(e){
		e=e||window.event;
		if(e.keyCode=="27"){
			layer.style.display = "none";
			this.isOpen = false;
		}
	}

	if(document.attachEvent){
		document.attachEvent("onkeyup",this.esc);
	}else{
		document.addEventListener("keyup",this.esc,true);
	}

}

var dragDrop = function(_obj){
	this.obj = _obj;
	this.x = 0;
	this.y = 0;
	this.init();
}
dragDrop.prototype = {
	getPos:function(_obj){
		var _x = _obj.offsetLeft,_y = _obj.offsetTop;
		while(_obj=_obj.offsetParent){
			_x += _obj.offsetLeft;
			_y += _obj.offsetTop;
		}
		_obj = null;
		return {x:_x,y:_y};
	},
	mDown:function(e){
		e = e||window.event;
		var pos = this.getPos(this.obj);
		this.x = e.clientX - pos.x;
		this.y = e.clientY - pos.y;
		pos = null;
		this.obj.style.position = "absolute";
		this.obj.style.zIndex = "99999999999";
		if(this.obj.setCapture){
			this.obj.setCapture();
		}else{
			e.preventDefault();
		}
		document.body.onmousemove = this.mMove.bind(this);
		document.body.onmouseup = this.mUp.bind(this);
	},
	mMove:function(e){
		e = e||window.event;
		_mx = (e.clientX - this.x);
		_my = (e.clientY - this.y);
		this.setPos(this.obj,_mx,_my);
	},
	mUp:function(e){
		e = e||window.event;
		this.x = e.clientX-this.x;
		this.y = e.clientY-this.y;
		this.setPos(this.obj,this.x,this.y);
		this.obj.style.zIndex = "999999999";
		if(this.obj.releaseCapture){
			this.obj.releaseCapture();
		}
		document.body.onmousemove = null;
		document.body.onmouseup = null;
	},
	setPos:function(_obj,x,y){
		_obj.style.left = x + "px";
		_obj.style.top = y + "px";
	},
	init:function(){
		if(Function.prototype.bind==undefined){
			Function.prototype.bind = function(_obj){
				var owner = this;
				return function(){
					owner.apply(_obj,arguments);
				}
			}
		};
		this.obj.onmousedown = this.mDown.bind(this);
	}
}
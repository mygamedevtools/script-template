/**
 * Module.cs
 * Created by: João Borks [joao.borks@gmail.com]
 * Created on: 2022-09-28
 */

using System.Collections.Generic;
using UnityEngine.UIElements;

namespace MyGameDevTools.ScriptTemplates.UIToolkit
{
    public class Module : BindableElement, INotifyValueChanged<bool>
    {
        public new class UxmlFactory : UxmlFactory<Module, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlStringAttributeDescription _titleAttribute = new UxmlStringAttributeDescription
            {
                name = "title",
                defaultValue = "New Module"
            };
            UxmlBoolAttributeDescription _enableableAttribute = new UxmlBoolAttributeDescription
            {
                name = "enableable",
                defaultValue = true
            };
            UxmlBoolAttributeDescription _valueAttribute = new UxmlBoolAttributeDescription
            {
                name = "value",
                defaultValue = true
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get
                {
                    yield return new UxmlChildElementDescription(typeof(VisualElement));
                }
            }

            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(element, bag, context);

                var module = (Module)element;
                module.title = _titleAttribute.GetValueFromBag(bag, context);
                module.enableable = _enableableAttribute.GetValueFromBag(bag, context);
                module.SetValueWithoutNotify(_valueAttribute.GetValueFromBag(bag, context));
            }
        }

        public override VisualElement contentContainer => _content;

        public string title
        {
            get => _title.text;
            set => _title.text = value;
        }
        public bool enableable
        {
            get => _toggle.visible;
            set => _toggle.visible = value;
        }
        public bool value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    using ChangeEvent<bool> changeEvent = ChangeEvent<bool>.GetPooled(_value, value);
                    changeEvent.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(changeEvent);
                }
            }
        }

        readonly VisualElement _content;
        readonly Toggle _toggle;
        readonly Label _title;

        bool _value;

        public Module()
        {
            var header = new VisualElement
            {
                name = "module-header"
            };
            header.AddToClassList("module-header");
            hierarchy.Add(header);

            var foldout = new Foldout();
            header.Add(foldout);

            _toggle = new Toggle()
            {
                value = true
            };
            _toggle.AddToClassList("module-header__toggle");
            _toggle.RegisterValueChangedCallback(evt =>
            {
                value = _toggle.value;
                evt.StopPropagation();
            });
            header.Add(_toggle);

            _title = new Label("New Module");
            _title.AddToClassList("module-header__title");
            header.Add(_title);

            _content = new VisualElement
            {
                name = "module-content"
            };
            _content.AddToClassList("module-content");
            _content.EnableInClassList("hidden", !foldout.value);
            hierarchy.Add(_content);

            foldout.RegisterValueChangedCallback(changeEvent => _content.EnableInClassList("hidden", !changeEvent.newValue));
        }

        public void SetValueWithoutNotify(bool newValue)
        {
            _value = newValue;
            _toggle.value = _value;
        }
    }
}
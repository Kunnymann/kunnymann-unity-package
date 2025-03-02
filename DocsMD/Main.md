# Quick start

개인적으로 작업한 Unity 패키지 공간이며, 다양한 기능을 이용해 손 쉽게 콘텐츠를 제작할 수 있습니다.

주로, 아래와 같은 플랫폼 기반 콘텐츠 제작을 타겟으로 하고 있습니다.

- 모바일 콘텐츠
- XR 콘텐츠
- 에디터 기능

## Environment

기본적으로 **Unity package manager** 포멧을 기반으로 하고 있으며, **Scoped registries** 를 사용하시길 권장드립니다.

- Unity 2022.3 LTS

## Installation

1. `Edit / Project Settings / Package Manager`에서 **Scoped Registries** 를 등록합니다.
2. URL 필드에 **http://ec2-43-200-170-179.ap-northeast-2.compute.amazonaws.com:4873/** 를 입력한 후, 해당 사이트에서 원하는 패키지들의 Scope를 확인하여 필드에 값을 채워넣습니다.
3. Registry를 등록한 후, Unity package manager에서 **Add package by name** 을 눌러, `com.kunnymann.[PACKAGE_NAME]`을 입력하여 패키지를 다운로드 받습니다.

## Content

| package       | namespace                | 설명                                                   |
|---------------|--------------------------|--------------------------------------------------------|
| Base          | com.kunnymann.base       | Module 동작을 지원하는 Core package                     |
| Navigation    | com.kunnymann.navigation | Navigation 기능을 지원하는 Module package               |
| Debugger      | com.kunnymann.debugger   | Debugger 기능과 ErrorListener 기능을 지원하는 Package    |
| UI Navigation | com.kunnymann.ui         | UI 기능을 지원하는 Package                              |

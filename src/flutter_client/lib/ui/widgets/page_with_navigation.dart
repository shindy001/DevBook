import 'package:flutter/material.dart';
import 'package:flutter_client/ui/theme/dev_book_spacing.dart';
import 'package:flutter_client/ui/widgets/responsive_layout.dart';
import 'package:flutter_client/ui/widgets/sidemenu.dart';

class PageWithNavigation extends StatelessWidget {
  const PageWithNavigation({super.key, required this.activeSideMenuItemName, required this.child});

  final String activeSideMenuItemName;
  final Widget child;

  @override
  Widget build(BuildContext context) {
    return ResponsiveLayoutBuilder(
      small: (_, __) => _pageViewComposition(
        expandedSideMenu: false,
        activeSideMenuItemName: activeSideMenuItemName,
        child: child,
      ),
      large: (_, __) => _pageViewComposition(
        expandedSideMenu: true,
        activeSideMenuItemName: activeSideMenuItemName,
        child: child,
      ),
    );
  }

  Widget _pageViewComposition({required bool expandedSideMenu, required String activeSideMenuItemName, required Widget child}) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        SideMenu(
          activeItem: activeSideMenuItemName,
          expanded: expandedSideMenu,
        ),
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(DevBookSpacing.md),
            child: child,
          ),
        ),
      ],
    );
  }
}

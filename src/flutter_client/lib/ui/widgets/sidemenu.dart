import 'package:flutter/material.dart';
import 'package:flutter_client/_app/router.dart';
import 'package:flutter_client/ui/layout/sizes.dart';
import 'package:flutter_client/ui/theme/theme.dart';
import 'package:gap/gap.dart';
import 'package:go_router/go_router.dart';
import 'package:quiver/strings.dart';

import '../theme/dev_book_colors.dart';

class SideMenu extends StatelessWidget {
  const SideMenu({
    super.key,
    this.activeItem,
    this.expanded = true,
  });

  final String? activeItem;
  final bool expanded;

  @override
  Widget build(BuildContext context) {
    return Container(
      color: DevBookColors.backgroundDark,
      width: expanded ? DevBookSizes.standardMenuWidth : DevBookSizes.compactMenuWidth,
      height: double.infinity,
      child: ListView(
        children: [
          const Gap(DevBookSpacing.xxxlg),
          Padding(
            padding: const EdgeInsets.only(top: DevBookSpacing.sm),
            child: SideMenuItem(
              itemName: 'Dashboard',
              activeItem: activeItem,
              icon: Icons.dashboard_rounded,
              showLabel: expanded,
              onTap: () => context.go(AppRoutes.dashboard),
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(top: DevBookSpacing.sm),
            child: SideMenuItem(
              itemName: 'App Setups',
              activeItem: activeItem,
              icon: Icons.app_registration_rounded,
              showLabel: expanded,
              onTap: () => context.go(AppRoutes.appSetups),
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(top: DevBookSpacing.sm),
            child: SideMenuItem(
              itemName: 'Startup Profiles',
              activeItem: activeItem,
              icon: Icons.developer_board,
              showLabel: expanded,
              onTap: () => context.go(AppRoutes.startupProfiles),
            ),
          ),
        ],
      ),
    );
  }
}

class SideMenuItem extends StatelessWidget {
  const SideMenuItem({
    required this.itemName,
    required this.activeItem,
    required this.icon,
    this.showLabel = true,
    this.onTap,
    super.key,
  });

  final String itemName;
  final String? activeItem;
  final bool showLabel;
  final IconData icon;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) {
    final isSelected = equalsIgnoreCase(itemName, activeItem);
    return ListTile(
      minVerticalPadding: 0,
      contentPadding: EdgeInsets.zero,
      selected: isSelected,
      title: Row(
        children: [
          Opacity(
            opacity: isSelected ? 1.0 : 0,
            child: Container(
              color: ListTileTheme.of(context).selectedColor,
              width: DevBookSizes.sideMenuActiveItemMarkerWidth,
              height: DevBookSizes.sideMenuItemHeight,
            ),
          ),
          const Gap(DevBookSpacing.md),
          Icon(icon),
          const Gap(DevBookSpacing.md),
          if (showLabel)
            Padding(
              padding: const EdgeInsets.only(bottom: DevBookSpacing.xs),
              child: Text(
                itemName,
                style: const TextStyle(fontWeight: FontWeight.w600),
              ),
            )
        ],
      ),
      onTap: onTap,
    );
  }
}

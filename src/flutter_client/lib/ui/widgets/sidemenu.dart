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
      color: DevBookColors.background,
      width: expanded ? DevBookSizes.standardMenuWidth : DevBookSizes.compactMenuWidth,
      height: double.infinity,
      child: ListView(
        children: [
          const Gap(DevBookSpacing.giga),
          SideMenuItem(
            itemName: 'Dashboard',
            activeItem: activeItem,
            icon: Icons.dashboard_rounded,
            showLabel: expanded,
            onTap: () => context.go(AppRoutes.dashboard),
          ),
          SideMenuItem(
            itemName: 'App Setups',
            activeItem: activeItem,
            icon: Icons.app_registration_rounded,
            showLabel: expanded,
            onTap: () => context.go(AppRoutes.appSetups),
          ),
          SideMenuItem(
            itemName: 'Startup Profiles',
            activeItem: activeItem,
            icon: Icons.developer_board,
            showLabel: expanded,
            onTap: () => context.go(AppRoutes.startupProfiles),
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
    return Container(
      margin: const EdgeInsets.all(DevBookSpacing.sm),
      child: Material(
        type: MaterialType.transparency,
        child: ListTile(
          selectedTileColor: DevBookColors.activeMenuItemBackground,
          splashColor: DevBookColors.transparent,
          selected: isSelected,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(DevBookSpacing.sm),
          ),
          contentPadding: const EdgeInsets.symmetric(horizontal: DevBookSpacing.sm),
          title: Row(
            children: [
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
        ),
      ),
    );
  }
}

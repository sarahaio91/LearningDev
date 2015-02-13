<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />
  <xsl:variable name="title" select="hf/@title" />

  <xsl:template match="/">
    <ncx xmlns="http://www.daisy.org/z3986/2005/ncx/" version="2005-1" xml:lang="nld">
      <head>
        <meta content="" name="dtb:uid" />
        <meta content="0" name="dtb:totalPageCount" />
        <meta content="0" name="dtb:maxPageNumber" />
      </head>
      <docTitle>
        <text>
          <xsl:value-of select="$title" />
        </text>
      </docTitle>
      <navMap>
        <navPoint>
          <navLabel>
            <text>Inhoud</text>
          </navLabel>
          <content src="Text/inhoud.html#calibre_toc_1" />
        </navPoint>
        <navPoint>
          <navLabel>
            <text>Colofon</text>
          </navLabel>
          <content src="Text/inhoud.html#calibre_toc_2" />
        </navPoint>
        <xsl:apply-templates select="//hfpart[@id]" />
        <navPoint>
          <navLabel>
            <text>Paginaregister</text>
          </navLabel>
          <content src="Text/artikelen.html#toclink_paginaregister" />
        </navPoint>
      </navMap>
    </ncx>
  </xsl:template>

  <xsl:template match="hfpart[@id]">
    <xsl:choose>
      <xsl:when test="not(descendant::hfpart)">
        <navPoint>
          <navLabel>
            <text>
              <xsl:value-of select="ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/titel" />
            </text>
          </navLabel>
          <content src="{concat('Text/artikelen.html#', generate-id(ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/titel))}" />
          <xsl:for-each select="pe">
            <xsl:if test="string-length(commentaarcontent/*/kopgeg)">
            <navPoint>
              <navLabel>
                <text>
                  <xsl:value-of select="commentaarcontent/*/kopgeg/*" />
                </text>
              </navLabel>
              <content src="{concat('Text/artikelen.html#', generate-id(commentaarcontent/*/kopgeg/*/text()))}" />
            </navPoint>
            </xsl:if>
          </xsl:for-each>
        </navPoint>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
